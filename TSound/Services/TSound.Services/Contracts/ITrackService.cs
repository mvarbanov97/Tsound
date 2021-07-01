using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface ITrackService : IService
    {
        Task<TrackServiceModel> GetTrackByIdAsync(Guid songId);

        Task<IEnumerable<TrackServiceModel>> GetAllTracksAsync();

        Task<IEnumerable<Track>> AddTracksToDbAsync(IEnumerable<SpotifyPlaylistModel.PlaylistTrack> tracks);

        Task<IEnumerable<TrackServiceModel>> GetTracksByPlaylistIdAsync(Guid playlistId);

        Task<IEnumerable<TrackServiceModel>> GetTop3TracksAsync();
    }
}
