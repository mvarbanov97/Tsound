using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TSound.Data.UnitOfWork;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;
using TSound.Services.Contracts;
using TSound.Services.Models;

namespace TSound.Services
{
    public class UserService : IUserService
    {
        public const int PageCountSize = 12;

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper, IHttpClientFactory clientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<UserServiceModel> GetUserByIdAsync(Guid userId)
        {
            var user = await this.unitOfWork.Users.All()
                .Include(u => u.Playlists)
                .Where(x => x.Id == userId).FirstOrDefaultAsync();

            return this.mapper.Map<UserServiceModel>(user);
        }

        public async Task<IEnumerable<UserServiceModel>> GetAllUsersAsync(bool isAdmin = false, int page = 1)
        {
            var users = await this.unitOfWork.Users.All()
                .Where(x => x.IsDeleted == false)
                .Include(u => u.Playlists)
                .ToListAsync();

            if (!isAdmin)
            {
                users = users.Where(x => x.IsBanned == false).ToList();
            }

            var usersByPage = users
                .Skip((page - 1) * PageCountSize)
                .Take(PageCountSize);

            return this.mapper.Map<IEnumerable<UserServiceModel>>(usersByPage);
        }

        public async Task<UserServiceModel> GetUserByEmailAsync(string email)
        {
            var user = await this.unitOfWork.Users.All().Where(x => x.Email == email).FirstOrDefaultAsync();

            return this.mapper.Map<UserServiceModel>(user);
        }

        public async Task<string> GetUserSpotifyId(string id)
        {
            var userLogins = this.unitOfWork.UserLogins.All().FirstOrDefault(x => x.UserId.ToString() == id);
            var spotifyId = userLogins.ProviderKey;

            return spotifyId;
        }

        public async Task<SpotifyUser> GetSpotifyUser(string id, string accessToken)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri($"https://api.spotify.com/v1/users/{id}");
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                  

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
            request.Headers.TryAddWithoutValidation("Content-Type", "application/json");
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                SpotifyUser spotifyUserData = JsonConvert.DeserializeObject<SpotifyUser>(result);

                return spotifyUserData;
            }

            return null;
        }

        public async Task<SpotifyUser> GetCurrentUserSpotifyProfile(string accessToken)
        {
            HttpClient client = new HttpClient();

            // TODO: Move all magic strings into Common project => GlobalConstants
            client.BaseAddress = new Uri($"https://api.spotify.com/v1/me"); 

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                SpotifyUser spotifyUserData = JsonConvert.DeserializeObject<SpotifyUser>(result);

                return spotifyUserData;
            }

            return null;
        }

        public int GetTotalUsersCount()
        {
            return this.unitOfWork.Users.All().Where(x => !x.IsDeleted).Count();
        }

        public int GetPageCountSizing()
        {
            int pageCountSize = PageCountSize;

            return pageCountSize;
        }
    }
}
