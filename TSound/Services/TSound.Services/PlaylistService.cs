using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.UnitOfWork;
using TSound.Plugin.Spotify.WebApi.Extensions;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.Models;
using static TSound.Plugin.Spotify.WebApi.SpotifyModels.SpotifyPlaylistModel;

namespace TSound.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IMapper mapper;
        private readonly HttpClient http;

        public PlaylistService(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper,
            HttpClient http)
        {
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
            this.mapper = mapper;
            this.http = http;
        }

        /// <summary>
        /// An async method that creates a playlist based on the information received in a PlaylistServiceModel that comes as a method parameter.
        /// </summary>
        /// <param name="playlistServiceModel">A PlaylistServiceModel that keeps the information of the playlist to be created.</param>
        /// <returns>A task that represents a PlaylistServiceModel that holds all information on the newly created user, including the new Guid id.</returns>
        public async Task<PlaylistServiceModel> CreatePlaylistAsync(PlaylistServiceModel playlistServiceModel)
        {
            playlistServiceModel.DateCreated = dateTimeProvider.GetDateTime();
            playlistServiceModel.DateModified = dateTimeProvider.GetDateTime();

            Playlist playlist = this.mapper.Map<Playlist>(playlistServiceModel);
            await this.unitOfWork.Playlists.AddAsync(playlist);

            await unitOfWork.CompleteAsync();

            var playlistJustCreated = this.unitOfWork.Playlists.All().First(x => x.Name == playlistServiceModel.Name);

            playlistServiceModel.Id = playlistJustCreated.Id;
            playlistServiceModel.SpotifyId = playlistJustCreated.SpotifyId;
            playlistServiceModel.DateCreated = playlistJustCreated.DateCreated;
            playlistServiceModel.DateModified = playlistJustCreated.DateModified;
            playlistServiceModel.IsDeleted = playlistJustCreated.IsDeleted;
            playlistServiceModel.IsUnlisted = playlistJustCreated.IsUnlisted;

            return playlistServiceModel;
        }

        public async Task AddTracksToPlaylist(Guid playlistId, IEnumerable<SpotifyPlaylistTrack> tracks)
        {
            var playlistToUpdate = await this.unitOfWork.Playlists.All().AsNoTracking().FirstOrDefaultAsync(x => x.Id == playlistId);

            foreach (var track in tracks)
            {
                var trackToAdd = await this.unitOfWork.Tracks.All().AsNoTracking().FirstOrDefaultAsync(x => x.SpotifyId == track.Track.Id);

                await this.unitOfWork.PlaylistTracks.AddAsync(new PlaylistTrack
                {
                    PlaylistId = playlistToUpdate.Id,
                    TrackId = trackToAdd.Id
                });
            }

            await this.unitOfWork.CompleteAsync();
        }

        public async Task AddCategoriesToPlaylist(Guid playlistId, IEnumerable<string> categoryIds)
        {
            var playlistToUpdate = await this.unitOfWork.Playlists.All().FirstOrDefaultAsync(x => x.Id == playlistId);

            foreach (var category in categoryIds)
            {
                var categoryToAdd = await this.unitOfWork.Categories.All().FirstOrDefaultAsync(x => x.SpotifyId == category);

                await this.unitOfWork.PlaylistCategories.AddAsync(new PlaylistCategory
                {
                    PlaylistId = playlistToUpdate.Id,
                    CategoryId = categoryToAdd.Id
                });
            }

            await this.unitOfWork.CompleteAsync();
        }

        /// <summary>
        /// An async method that deletes a playlist if a playlist with such id (from the parameter of the method) exists.
        /// </summary>
        /// <param name="playlistId">A Guid that is the id of the playlist to delete.</param>
        /// <param name="isApiKeyRequired">A boolean that reveals if the method is called from an ApiController and if API Key is needed to proceed.</param>
        /// <param name="userApiKey">A Guid? that is the API Key of the caller if method is called from an ApiController.</param>
        /// <returns>A task that represents a boolean that reveals if the action is successfully achieved.</returns>
        public async Task<bool> DeletePlaylistAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null)
        {
            this.ValidatePlaylistId(playlistId);
            this.ValidateIfPlaylistIsUnlisted(playlistId);
            this.ValidateIfPlaylistIsDeleted(playlistId);

            User userWithApiKey = null;

            if (isApiKeyRequired)
            {
                userWithApiKey = await this.ValidateAPIKeyAsync(userApiKey, this.unitOfWork);
                this.ValidateIfUserWithThisApiKeyIsTheAuthorOfPlaylist(playlistId, userWithApiKey);
            }

            var playlistInDb = await this.unitOfWork.Playlists.All().FirstAsync(x => x.Id == playlistId);

            playlistInDb.DateModified = dateTimeProvider.GetDateTime();
            playlistInDb.IsDeleted = true;

            await unitOfWork.CompleteAsync();
            return true;
        }

        /// <summary>
        /// An async method that gets all playlists within the database that are not deleted.
        /// </summary>
        /// <param name="isApiKeyRequired">A boolean that reveals if the method is called from an ApiController and if API Key is needed to proceed.</param>
        /// <param name="userApiKey">A Guid? that is the API Key of the caller if method is called from an ApiController.</param>
        /// <param name="isAdmin">A boolean that reveals if the method is called from an Admin or not.</param>
        /// <returns>A task that represents a collection of PlaylistServiceModels with all playlists in the database that are not deleted.</returns>
        public async Task<IEnumerable<PlaylistServiceModel>> GetAllPlaylistsAsync(bool isApiKeyRequired = false, Guid? userApiKey = null, bool isAdmin = false)
        {
            if (isApiKeyRequired)
                await this.ValidateAPIKeyAsync(userApiKey, this.unitOfWork);

            var playlists = await this.unitOfWork.Playlists.All()
                .Where(x => x.IsDeleted == false)
                .Where(x => x.DurationTravel != 0)
                .Include(x => x.User)
                .Include(x => x.Categories)
                .Include(x => x.Tracks)
                .ThenInclude(playlistSong => playlistSong.Track)
                .ToListAsync();

            if (!isAdmin)
            {
                playlists = playlists.Where(x => x.IsUnlisted == false).ToList();
            }

            var result = this.mapper.Map<IEnumerable<PlaylistServiceModel>>(playlists);
            return result;
        }

        public async Task<PlaylistServiceModel> GetPlaylistByIdAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null)
        {
            if (isApiKeyRequired)
                await this.ValidateAPIKeyAsync(userApiKey, this.unitOfWork);

            this.ValidatePlaylistId(playlistId);
            this.ValidateIfPlaylistIsDeleted(playlistId);

            Playlist playlist = await this.unitOfWork.Playlists.All()
                .Include(x => x.User)
                .Include(x => x.Categories)
                .Include(x => x.Tracks)
                .ThenInclude(playlistSong => playlistSong.Track)
                .FirstOrDefaultAsync(p => p.Id == playlistId);

            PlaylistServiceModel playlistServiceModel = this.mapper.Map<PlaylistServiceModel>(playlist);

            return playlistServiceModel;
        }

        public async Task<IEnumerable<PlaylistServiceModel>> GetPlaylistsByUserIdAsync(Guid userId)
        {
            if (!this.unitOfWork.Users.All().Any(x => x.Id == userId))
            {
                throw new ArgumentException("You are trying to get the playlists of a user with an Id that does not exist in the database.");
            }

            if (!unitOfWork.Playlists.All().Any(x => x.UserId == userId))
            {
                return new List<PlaylistServiceModel>();
            }
            else
            {
                var playlistsOfThisUser = await unitOfWork.Playlists.All()
                    .Where(x => x.UserId == userId)
                    .Where(x => x.IsDeleted == false)
                    .Where(x => x.DurationTravel != 0)
                    .Include(x => x.Categories)
                    .Include(x => x.Tracks)
                    .ThenInclude(playlistSong => playlistSong.Track)
                    .ToListAsync();

                return this.mapper.Map<IEnumerable<PlaylistServiceModel>>(playlistsOfThisUser);
            }
        }

        public async Task<IEnumerable<SpotifyPlaylistTrack>> GenerateTracksForPlaylistAsync(int durationTravel, IEnumerable<string> categoryIdsToUse, string userAccessToken)
        {
            Random random = new Random();
            int durationDubbedCurrent = 0;
            var trackUrisToAdd = new List<string>(100);
            var tracks = new List<SpotifyPlaylistTrack>();

            // If for some reason we don't receive any Genres by the User - use all available Genres.
            if (categoryIdsToUse == null || categoryIdsToUse.Count() == 0)
            {
                categoryIdsToUse = this.unitOfWork.Categories.All().Select(x => x.SpotifyId);
            }

            while (true)
            {

                if (durationTravel <= durationDubbedCurrent)
                {
                    break;
                }

                string randomCategoryId = categoryIdsToUse.Skip(random.Next(0, categoryIdsToUse.Count())).First();

                var randomPlaylist = await this.GetCategoryPlaylists<PagedPlaylists>(randomCategoryId, "BG", 1, random.Next(1, 20), userAccessToken);

                var randomTracksOfPlaylist = await this.GetPlaylistTracks<PlaylistPaged>(randomPlaylist.Items[0].Id, userAccessToken, null, random.Next(1, 3), random.Next(1, 20));

                var tracksToAdd = randomTracksOfPlaylist.Items.ToList();

                tracksToAdd.Select(x => { x.Track.SpotifyCategoryId = randomCategoryId; return x; }).ToList();

                foreach (var track in tracksToAdd)
                {
                    if (!tracks.Select(x => x.Track.Name).Contains(track.Track.Name))
                    {
                        tracks.Add(track);
                        durationDubbedCurrent += track.Track.DurationMs;
                    }
                }

            }
            return tracks;
        }

        public async Task<bool> UpdatePlaylistDurationTravelAsync(Guid id, int durationTravel)
        {
            this.ValidatePlaylistId(id);
            this.ValidateIfPlaylistIsDeleted(id);

            this.ValidateDurationTravel(durationTravel);

            var playlist = await this.unitOfWork.Playlists.All().FirstAsync(x => x.Id == id);

            playlist.DurationTravel = durationTravel;
            await this.unitOfWork.CompleteAsync();
            return true;
        }

        public async Task UpdatePlaylistCoverImageAsync(Guid playlistId, string coverImageUrl)
        {
            var playlist = await this.unitOfWork.Playlists.All().FirstAsync(x => x.Id == playlistId);

            playlist.Image = coverImageUrl;

            await this.unitOfWork.CompleteAsync();
        }

        public async Task UpdatePlaylistSongsCountAsync(Guid playlistId, int count)
        {
            var playlist = await this.unitOfWork.Playlists.All().FirstAsync(x => x.Id == playlistId);

            playlist.SongsCount = count;

            await this.unitOfWork.CompleteAsync();
        }

        public async Task<T> GetCategoryPlaylists<T>(
            string categoryId,
            string country = null,
            int? limit = null,
            int offset = 0,
            string accessToken = null)
        {
            var baseUrl = $"https://api.spotify.com/v1/browse/categories/{categoryId}/playlists";

            var builder = new UriBuilder($"{baseUrl}");
            builder.AppendToQueryIfValueNotNullOrWhiteSpace("country", country);
            builder.AppendToQueryIfValueGreaterThan0("limit", limit);
            builder.AppendToQueryIfValueGreaterThan0("offset", limit);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);

            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);

            var response = await this.http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                JObject deserialized = JsonConvert.DeserializeObject(result) as JObject;
                var playlists = deserialized["playlists"].ToObject<T>();

                return playlists;
            }

            return default(T);
        }

        public async Task<T> GetPlaylistTracks<T>(
            string playlistId,
            string accessToken = null,
            string fields = null,
            int? limit = null,
            int offset = 0,
            string market = null,
            string[] additionalTypes = null)
        {
            if (string.IsNullOrEmpty(playlistId)) throw new ArgumentNullException(nameof(playlistId));

            var builder = new UriBuilder($"https://api.spotify.com/v1/playlists/{playlistId}/tracks");
            builder.AppendToQueryIfValueNotNullOrWhiteSpace("fields", fields);
            builder.AppendToQueryIfValueGreaterThan0("limit", limit);
            builder.AppendToQueryIfValueGreaterThan0("offset", offset);
            builder.AppendToQueryIfValueNotNullOrWhiteSpace("market", market);
            builder.AppendToQueryAsCsv("additional_types", additionalTypes);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);
            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);

            var response = await this.http.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var playlistTracks = JsonConvert.DeserializeObject<T>(result);

                return playlistTracks;
            }

            return default(T);
        }

        public async Task<IEnumerable<PlaylistServiceModel>> Get3RandomPlaylists()
        {
            var random = new Random();
            var threeRandomPlaylists = await this.unitOfWork.Playlists.All().Skip(random.Next(1, 5)).Take(3).ToListAsync();

            var result = this.mapper.Map<IEnumerable<PlaylistServiceModel>>(threeRandomPlaylists);
            return result;
        }

        private void ValidateIfUserWithThisApiKeyIsTheAuthorOfPlaylist(Guid playlistId, User user)
        {
            ValidatePlaylistId(playlistId);

            var playlist = this.unitOfWork.Playlists.All().FirstOrDefault(x => x.Id == playlistId);

            if (playlist.UserId != user.Id)
            {
                throw new InvalidOperationException("The current User is not the author of the Playlist so no operations with it are allowed.");
            }
        }

        private void ValidatePlaylistId(Guid id)
        {
            if (!this.unitOfWork.Playlists.All().Any(x => x.Id == id))
            {
                throw new ArgumentException("A Playlist with such Id does not exist in the database.");
            }
        }

        private void ValidateDurationTravel(int durationTravel)
        {
            if (durationTravel < 10 /*minutes*/ * 60 /*seconds*/)
            {
                throw new ArgumentException("The duration of your travel cannot be less than 10 minutes.");
            }
        }

        public void ValidateIfPlaylistIsDeleted(Guid id)
        {
            if (this.unitOfWork.Playlists.All().First(x => x.Id == id).IsDeleted)
            {
                throw new ArgumentException("Playlist is deleted.");
            }
        }

        private void ValidateIfPlaylistIsUnlisted(Guid id)
        {
            if (this.unitOfWork.Playlists.All().First(x => x.Id == id).IsUnlisted)
            {
                throw new ArgumentException("Playlist is unlisted.");
            }
        }
    }
}
