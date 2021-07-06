using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface IUserService : IService
    {
        Task<UserServiceModel> GetUserByIdAsync(Guid id);

        Task<IEnumerable<UserServiceModel>> GetAllUsersAsync(bool isAdmin = false, int page = 1);

        Task<UserServiceModel> GetUserByEmailAsync(string email);

        Task<string> GetUserSpotifyId(string id);

        Task<SpotifyUser> GetSpotifyUser(string id, string accessToken);

        Task<SpotifyUser> GetCurrentUserSpotifyProfile(string accessToken);

        int GetTotalUsersCount();

        int GetPageCountSizing();
    }
}
