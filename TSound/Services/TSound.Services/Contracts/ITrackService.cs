using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Services.Models;
using static TSound.Plugin.Spotify.WebApi.SpotifyModels.SpotifyPlaylistModel;

namespace TSound.Services.Contracts
{
    public interface ITrackService : IService
    {
        Task<TrackServiceModel> GetTrackByIdAsync(Guid songId);

        Task<IEnumerable<TrackServiceModel>> GetAllTracksAsync();

        Task<IEnumerable<Track>> AddTracksToDbAsync(IEnumerable<SpotifyPlaylistTrack> tracks);

        Task<IEnumerable<TrackServiceModel>> GetTracksByPlaylistIdAsync(Guid playlistId);

        Task<IEnumerable<TrackServiceModel>> GetTop3TracksAsync();
    }
}
