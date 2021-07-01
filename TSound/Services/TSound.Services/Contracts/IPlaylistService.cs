using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Services.Models;
using static TSound.Data.Models.SpotifyDomainModels.SpotifyPlaylistModel;

namespace TSound.Services.Contracts
{
    public interface IPlaylistService : IService
    {
        Task<PlaylistServiceModel> CreatePlaylistAsync(PlaylistServiceModel playlistServiceModel);

        Task AddTracksToPlaylist(Guid playlistId, IEnumerable<SpotifyPlaylistModel.PlaylistTrack> tracks);

        Task AddCategoriesToPlaylist(Guid playlistId, IEnumerable<string> categories);

        Task<bool> DeletePlaylistAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<IEnumerable<PlaylistServiceModel>> GetAllPlaylistsAsync(bool isApiKeyRequired = false, Guid? userApiKey = null, bool isAdmin = false);

        Task<PlaylistServiceModel> GetPlaylistByIdAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<IEnumerable<PlaylistServiceModel>> GetPlaylistsByUserIdAsync(Guid userId);

        Task<IEnumerable<PlaylistTrack>> GenerateTracksForPlaylistAsync(int durationTravel, IEnumerable<string> categoryIdsToUse, string userAccessToken);

        Task<bool> UpdatePlaylistDurationTravelAsync(Guid id, int durationTravel);

        Task UpdatePlaylistCoverImageAsync(Guid playlistId, string coverImageUrl);

        Task UpdatePlaylistSongsCountAsync(Guid playlistId, int count);

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

        Task<IEnumerable<PlaylistServiceModel>> Get3RandomPlaylists();
    }
}
