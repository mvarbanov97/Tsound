using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.UnitOfWork;
using TSound.Plugin.Spotify.WebApi.Contracts;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.Models;

namespace TSound.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IBrowseApi browseApi;

        public CategoryService()
        {

        }

        public CategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBrowseApi browseApi)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.browseApi = browseApi;
        }

        public async Task<CategoryServiceModel> GetCategoryByIdAsync(Guid categoryId)
        {
            var category = await this.unitOfWork.Categories.All().FirstOrDefaultAsync(x => x.Id == categoryId);

            if (category == null)
                throw new ArgumentNullException("Category Not Found.");

            var categoryServiceModel = this.mapper.Map<CategoryServiceModel>(category);

            return categoryServiceModel;
        }
        
        public async Task<IEnumerable<CategoryServiceModel>> GetCategoryByPlaylistIdAsync(Guid playlistId)
        {
            if (!this.unitOfWork.Playlists.All().Any(x => x.Id == playlistId))
            {
                throw new ArgumentNullException("There is no playlist with such Id in the database.");
            }

            var categoriesFromThisPlaylistServiceModels = await this.unitOfWork.PlaylistCategories.All()
                .Where(x => x.PlaylistId == playlistId)
                .Select(x => this.unitOfWork.Categories.All().First(y => y.Id == x.CategoryId))
                .ToListAsync();

            return this.mapper.Map<IEnumerable<CategoryServiceModel>>(categoriesFromThisPlaylistServiceModels);
        }

        public async Task<IEnumerable<CategoryServiceModel>> GetAllCategoriesAsync(bool requireApiKey = false, System.Guid? apiKey = null)
        {
            if (requireApiKey)
                await this.ValidateAPIKeyAsync(apiKey, this.unitOfWork);

            var genres = this.unitOfWork.Categories.All();

            if (genres == null)
                return new List<CategoryServiceModel>();

            var result = this.mapper.Map<IEnumerable<CategoryServiceModel>>(genres);

            return result;
        }

        public async Task LoadCategoriesInDb()
        {
            if (this.DatabaseContainsCategories())
                return;

            var categories = await this.browseApi.GetCategories<PagedCategories>();

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

        private bool DatabaseContainsCategories()
        {
            if (this.unitOfWork.Categories == null || this.unitOfWork.Categories.All().Count() == 0)
                return false;

            return true;
        }
    }
}