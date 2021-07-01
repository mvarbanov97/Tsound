using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.Models;

namespace TSound.Services
{
    public class TrackService : ITrackService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrackService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<TrackServiceModel> GetTrackByIdAsync(Guid trackId)
        {
            var song = await this.unitOfWork.Tracks.All()
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == trackId);

            if (song == null)
                throw new ArgumentNullException("Track Not Found.");

            var songServiceModel = this.mapper.Map<TrackServiceModel>(song);

            return songServiceModel;
        }

        public async Task<IEnumerable<TrackServiceModel>> GetAllTracksAsync()
        {
            var songs = this.unitOfWork.Tracks.All()
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Include(s => s.Category);

            var songServiceModels = this.mapper.Map<IEnumerable<TrackServiceModel>>(songs);

            return songServiceModels;
        }

        public async Task<IEnumerable<Track>> AddTracksToDbAsync(IEnumerable<SpotifyPlaylistModel.PlaylistTrack> playlistTracks)
        {
            var tracks = playlistTracks.Select(x => x.Track).ToList();

            var tracksToAdd = this.mapper.Map<IEnumerable<Track>>(tracks);

            foreach (var track in tracksToAdd)
            {
                var category = await this.unitOfWork.Categories.All().FirstOrDefaultAsync(x => x.SpotifyId == track.SpotifyCategoryId);
                track.CategoryId = category.Id;
                await this.unitOfWork.Tracks.AddAsync(track);
            }

            await this.unitOfWork.CompleteAsync();

            return tracksToAdd;
        }

        public async Task<IEnumerable<TrackServiceModel>> GetTracksByPlaylistIdAsync(Guid playlistId)
        {
            var tracks = this.unitOfWork.PlaylistTracks.All()
                .Where(s => s.PlaylistId == playlistId)
                .Include(t => t.Track)
                .ThenInclude(t => t.Album)
                .Include(t => t.Track)
                .ThenInclude(t => t.Artist)
                .Include(t => t.Track)
                .ThenInclude(t => t.Category)
                .Select(t => t.Track);

            var songServiceModel = this.mapper.Map<IEnumerable<TrackServiceModel>>(tracks);

            return await Task.FromResult(songServiceModel);
        }

        public async Task<IEnumerable<TrackServiceModel>> GetTop3TracksAsync()
        {
            var random = new Random();

            var tracks = await this.unitOfWork.Tracks.All()
                .AsNoTracking()
                .Skip(random.Next(1, 10))
                .Take(3)
                .ToListAsync();

            var trackServiceModel = this.mapper.Map<IEnumerable<TrackServiceModel>>(tracks);

            return trackServiceModel;
        }
    }
}
