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
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Models;

namespace TSound.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper, IHttpClientFactory clientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserServiceModel>> GetAllUsersAsync()
        {
            var users = await this.unitOfWork.Users.All().ToListAsync();

            return users.Select(user => new UserServiceModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateCreated = user.DateCreated,
                DateModified = user.DateModified,
                Image = user.ImageUrl,
                IsAdmin = user.IsAdmin,
                IsBanned = user.IsBanned,
                IsDeleted = user.IsDeleted,
            });
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

        public async Task<UserSpotify> GetSpotifyUser(string id, string accessToken)
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
                UserSpotify spotifyUserData = JsonConvert.DeserializeObject<UserSpotify>(result);

                return spotifyUserData;
            }

            return null;
        }
    }
}
