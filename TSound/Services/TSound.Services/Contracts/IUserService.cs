using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface IUserService : IService
    {
        Task<IEnumerable<UserServiceModel>> GetAllUsersAsync();

        Task<UserServiceModel> GetUserByEmailAsync(string email);

        Task<string> GetUserSpotifyId(string id);

        Task<UserSpotify> GetSpotifyUser(string id, string accessToken);
    }
}
