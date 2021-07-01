using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.External.SpotifyAuthorization;
using TSound.Services.Models;

namespace TSound.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private HttpClient http;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient http, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.http = http;
            this.configuration = configuration;
        }

        public async Task<GenreServiceModel> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await this.unitOfWork.Categories.All().FirstOrDefaultAsync(x => x.Id == categoryId);

            if (category == null)
                throw new ArgumentNullException("Category Not Found.");

            var categoryServiceModel = this.mapper.Map<GenreServiceModel>(category);

            return categoryServiceModel;
        }
        
        public async Task<IEnumerable<GenreServiceModel>> GetCategoryByPlaylistIdAsync(Guid playlistId)
        {
            var categoryFromThisPlaylistServiceModels = await this.unitOfWork.PlaylistCategories.All()
                .Where(x => x.PlaylistId == playlistId)
                .Select(x => this.unitOfWork.Categories.All().First(y => y.Id == x.CategoryId))
                .ToListAsync();

            return this.mapper.Map<IEnumerable<GenreServiceModel>>(categoryFromThisPlaylistServiceModels);
        }

        public async Task<IEnumerable<GenreServiceModel>> GetAllCategoriesAsync(bool requireApiKey = false, System.Guid? apiKey = null)
        {
            if (requireApiKey)
                await this.ValidateAPIKeyAsync(apiKey, this.unitOfWork);

            var genres = this.unitOfWork.Categories.All();

            if (genres == null)
                return new List<GenreServiceModel>();

            var result = this.mapper.Map<IEnumerable<GenreServiceModel>>(genres);

            return result;
        }

        public async Task LoadCategoriesInDb()
        {
            if (this.DatabaseContainsCategories())
                return;

            var categories = await this.GetSpotifyCategoriesFromApi<PagedCategories>();

            foreach (var category in categories.Items)
            {
                Category genre = new Category
                {
                    SpotifyId = category.Id,
                    Name = category.Name,
                    Link = category.Href,
                    ImageUrl = category.Icons.First().Url,
                };

                await this.unitOfWork.Categories.AddAsync(genre);
            }

            await this.unitOfWork.CompleteAsync();
        }

        public async Task<T> GetSpotifyCategoriesFromApi<T>()
        {
            var accounts = new AccountsService(http, configuration);
            var token = await accounts.GetAccessToken();
            var client = new HttpClient();
            client.BaseAddress = new Uri($"https://api.spotify.com/v1/browse/categories");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            request.Headers.TryAddWithoutValidation("limit", "10");
            request.Headers.TryAddWithoutValidation("offset", "0");
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + token);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                JObject deserialized = JsonConvert.DeserializeObject(result) as JObject;
                var categories = deserialized["categories"].ToObject<T>();

                return categories;
            }
            else
                throw new HttpRequestException("Invalid request sent to the Spotify API.");
        }

        private bool DatabaseContainsCategories()
        {
            if (this.unitOfWork.Categories == null || this.unitOfWork.Categories.All().Count() == 0)
                return false;

            return true;
        }
    }
}