using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface IPlaylistService : IService
    {
        Task<PlaylistServiceModel> CreatePlaylistAsync(PlaylistServiceModel playlistServiceModel);

        Task<bool> DeletePlaylistAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<int> GetDurationTravelAsync(string queryFirst, string querySecond);

        Task<IEnumerable<PlaylistServiceModel>> GetAllPlaylistsAsync(bool isApiKeyRequired = false, Guid? userApiKey = null, bool isAdmin = false);

        Task<PlaylistServiceModel> GetPlaylistByIdAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<IEnumerable<PlaylistServiceModel>> GetPlaylistsByUserIdAsync(Guid userId);

        Task<bool> GeneratePlaylistAsync(Guid playlistId, int durationTravel, IEnumerable<Guid> genresToUse, bool IsTopTracksOptionEnabled, bool IsTracksFromSameArtistEnabled);

        Task<bool> UpdatePlaylistDurationTravelAsync(Guid id, int durationTravel);
    }
}
