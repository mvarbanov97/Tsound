using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Models;
using static TSound.Plugin.Spotify.WebApi.SpotifyModels.SpotifyPlaylistModel;

namespace TSound.Services.Contracts
{
    public interface IPlaylistService : IService
    {
        Task<PlaylistServiceModel> CreatePlaylistAsync(PlaylistServiceModel playlistServiceModel);

        Task AddTracksToPlaylist(Guid playlistId, IEnumerable<SpotifyPlaylistTrack> tracks);

        Task AddCategoriesToPlaylist(Guid playlistId, IEnumerable<string> categories);

        Task<PlaylistServiceModel> UpdatePlaylistAsync(PlaylistServiceModel playlistToUpdate);

        Task<bool> DeletePlaylistAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<IEnumerable<PlaylistServiceModel>> GetAllPlaylistsAsync(bool isApiKeyRequired = false, Guid? userApiKey = null, bool isAdmin = false);

        Task<PlaylistServiceModel> GetPlaylistByIdAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null);

        Task<IEnumerable<PlaylistServiceModel>> GetPlaylistsByUserIdAsync(Guid userId);

        Task<IEnumerable<SpotifyPlaylistTrack>> GenerateTracksForPlaylistAsync(int durationTravel, IEnumerable<string> categoryIdsToUse, string userAccessToken);

        Task<bool> UpdatePlaylistDurationTravelAsync(Guid id, int durationTravel);

        Task UpdatePlaylistCoverImageAsync(Guid playlistId, string coverImageUrl);

        Task UpdatePlaylistSongsCountAsync(Guid playlistId, int count);

        Task<IEnumerable<PlaylistServiceModel>> Get3RandomPlaylists();

        Task<IEnumerable<PlaylistServiceModel>> GetPlaylistsByContainsSubstring(string substring);

        IEnumerable<PlaylistServiceModel> FilterByRange(IEnumerable<PlaylistServiceModel> collectionToFilter, string filterMethod, int min, int max);

        IEnumerable<PlaylistServiceModel> FilterByCategory(IEnumerable<PlaylistServiceModel> playlists, IEnumerable<Guid> genresIdsChosenByUser);

        IEnumerable<PlaylistServiceModel> Sort(IEnumerable<PlaylistServiceModel> collectionToFilter, string sortMethod, string sortOrder);
    }
}
