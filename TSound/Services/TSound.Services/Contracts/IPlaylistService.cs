using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Models;
using static TSound.Data.Models.SpotifyDomainModels.SpotifyPlaylistModel;

namespace TSound.Services.Contracts
{
    public interface IPlaylistService : IService
    {
        Task<PlaylistServiceModel> CreatePlaylistAsync(PlaylistServiceModel playlistServiceModel);

        Task<bool> DeletePlaylistAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<IEnumerable<PlaylistServiceModel>> GetAllPlaylistsAsync(bool isApiKeyRequired = false, Guid? userApiKey = null, bool isAdmin = false);

        Task<PlaylistServiceModel> GetPlaylistByIdAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<IEnumerable<PlaylistServiceModel>> GetPlaylistsByUserIdAsync(Guid userId);

        Task<string[]> GeneratePlaylistAsync(Guid playlistId, int durationTravel, IEnumerable<string> genresToUse, string userAcessToken);

        Task<bool> UpdatePlaylistDurationTravelAsync(Guid id, int durationTravel);

        Task<T> GetPlaylistTracks<T>(string playlistId,
            string accessToken = null,
            string fields = null,
            int? limit = null,
            int offset = 0,
            string market = null,
            string[] additionalTypes = null);

        Task<T> GetCategoryPlaylists<T>(
            string categoryId,
            string country = null,
            int? limit = null,
            int offset = 0,
            string accessToken = null);
    }
}
