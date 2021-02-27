using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.Models;
using TSound.Services.Validators;

namespace TSound.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceValidator validator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IMapper mapper;

        public PlaylistService(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
            this.mapper = mapper;
        }

        /// <summary>
        /// An async method that creates a playlist based on the information received in a PlaylistServiceModel that comes as a method parameter.
        /// </summary>
        /// <param name="playlistServiceModel">A PlaylistServiceModel that keeps the information of the playlist to be created.</param>
        /// <returns>A task that represents a PlaylistServiceModel that holds all information on the newly created user, including the new Guid id.</returns>
        public async Task<PlaylistServiceModel> CreatePlaylistAsync(PlaylistServiceModel playlistServiceModel)
        {
            this.validator.ValidateIfNameIsNullOrEmpty(playlistServiceModel.Name);
            this.validator.ValidateIfAuthorExistsInDb(playlistServiceModel.UserId);
            this.validator.ValidateIfAuthorIsDeleted(playlistServiceModel.UserId);


            playlistServiceModel.DateCreated = dateTimeProvider.GetDateTime();
            playlistServiceModel.DateModified = dateTimeProvider.GetDateTime();

            Playlist playlist = this.mapper.Map<Playlist>(playlistServiceModel);
            await this.unitOfWork.Playlists.AddAsync(playlist);

            await unitOfWork.CompleteAsync();

            var playlistJustCreated = this.unitOfWork.Playlists.All().First(x => x.Name == playlistServiceModel.Name);

            playlistServiceModel.Id = playlistJustCreated.Id;
            playlistServiceModel.DateCreated = playlistJustCreated.DateCreated;
            playlistServiceModel.DateModified = playlistJustCreated.DateModified;
            playlistServiceModel.IsDeleted = playlistJustCreated.IsDeleted;
            playlistServiceModel.IsUnlisted = playlistJustCreated.IsUnlisted;

            return playlistServiceModel;
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
            this.validator.ValidatePlaylistId(playlistId);
            this.validator.ValidateIfPlaylistIsUnlisted(playlistId);
            this.validator.ValidateIfPlaylistIsDeleted(playlistId);

            User userWithApiKey = null;

            if (isApiKeyRequired)
            {
                userWithApiKey = await this.ValidateAPIKeyAsync(userApiKey, this.unitOfWork);
                this.validator.ValidateIfUserWithThisApiKeyIsTheAuthorOfPlaylist(playlistId, userWithApiKey);
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
                .Include(x => x.Genres)
                .Include(x => x.Songs)
                .ThenInclude(playlistSong => playlistSong.Song)
                .ToListAsync();

            if (!isAdmin)
            {
                playlists = playlists.Where(x => x.IsUnlisted == false).ToList();
            }

            var result = this.mapper.Map<IEnumerable<PlaylistServiceModel>>(playlists);
            return result;
        }

        public async Task<int> GetDurationTravelAsync(string queryFirst, string querySecond)
        {
            this.validator.ValidateIfNameIsNullOrEmpty(queryFirst);
            this.validator.ValidateIfNameIsNullOrEmpty(querySecond);

            List<double> coordinatesFirstPoint = await this.GetLocationByQueryAsync(queryFirst);
            List<double> coordinatesSecondPoint = await this.GetLocationByQueryAsync(querySecond);

            if (coordinatesFirstPoint == null || coordinatesSecondPoint == null)
            {
                throw new ArgumentException("You did not get coordinates.");
            }

            int duration = await GetDurationByCoordinatesAsync(coordinatesFirstPoint, coordinatesSecondPoint) * 60 /*seconds*/;

            this.validator.ValidateDurationTravel(duration);

            return duration;
        }

        public async Task<PlaylistServiceModel> GetPlaylistByIdAsync(Guid playlistId, bool isApiKeyRequired = false, Guid? userApiKey = null)
        {
            if (isApiKeyRequired)
                await this.ValidateAPIKeyAsync(userApiKey, this.unitOfWork);

            this.validator.ValidatePlaylistId(playlistId);
            this.validator.ValidateIfPlaylistIsDeleted(playlistId);

            Playlist playlist = await this.unitOfWork.Playlists.All()
                .Include(x => x.User)
                .Include(x => x.Genres)
                .Include(x => x.Songs)
                .ThenInclude(playlistSong => playlistSong.Song)
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
                    .Include(x => x.Genres)
                    .Include(x => x.Songs)
                    .ThenInclude(playlistSong => playlistSong.Song)
                    .ToListAsync();

                return this.mapper.Map<IEnumerable<PlaylistServiceModel>>(playlistsOfThisUser);
            }
        }

        public async Task<bool> GeneratePlaylistAsync(Guid playlistId, int durationTravel, IEnumerable<Guid> genresIdsToUse, bool IsTopTracksOptionEnabled, bool IsTracksFromSameArtistEnabled)
        {
            this.validator.ValidatePlaylistId(playlistId);
            this.validator.ValidateDurationTravel(durationTravel);

            Random random = new Random();
            HashSet<Guid> listArtistsIdsAlreadyUsed = new HashSet<Guid>();

            int durationDubbedCurrent = 0;

            // If for some reason we don't receive any Genres by the User - use all available Genres.
            if (genresIdsToUse == null || genresIdsToUse.Count() == 0)
            {
                genresIdsToUse = this.unitOfWork.Genres.All().Select(x => x.Id);
            }

            while (true)
            {
                // If Duration Travel - 5 min <= Duration Playlist Current < Duration Travel + 5 min => break.
                if (durationDubbedCurrent >= durationTravel - (5 * 60) && durationDubbedCurrent <= durationTravel + (5 * 60))
                {
                    break;
                }

                Guid randomGenreId = genresIdsToUse.Skip(random.Next(0, genresIdsToUse.Count())).First();

                IEnumerable<Guid> collectionSongsIdsOfThisGenre = new List<Guid>();

                // Take all songs from the random Genre and if "Is Top Tracks Option Enabled" => take only those of Rank 100.000.
                if (IsTopTracksOptionEnabled)
                {
                    collectionSongsIdsOfThisGenre = this.unitOfWork.Songs.All().Where(x => x.GenreId == randomGenreId).Where(x => x.Rank <= 500000).Select(x => x.Id);
                }
                else
                {
                    collectionSongsIdsOfThisGenre = this.unitOfWork.Songs.All().Where(x => x.GenreId == randomGenreId).Select(x => x.Id);
                }

                // Take a random Song Id - skipping a random count of songs.
                Guid randomSongId = collectionSongsIdsOfThisGenre.Skip(random.Next(0, collectionSongsIdsOfThisGenre.Count())).First();
                Song randomSong = this.unitOfWork.Songs.All().First(x => x.Id == randomSongId);

                // If "Is Tracks From Same Artist Enabled" and the current list of "used" Artists contains the author of the randomly chosen Song => continue.
                if (IsTracksFromSameArtistEnabled && listArtistsIdsAlreadyUsed.Any(x => x == randomSong.ArtistId))
                {
                    continue;
                }

                // If the random Genre does not exist in the Playlist<>Genre table => Add it.
                if (!this.unitOfWork.PlaylistsGenres.All().Where(x => x.PlaylistId == playlistId).Any(b => b.GenreId == randomGenreId))
                {
                    await this.unitOfWork.PlaylistsGenres.AddAsync(new PlaylistGenre
                    {
                        PlaylistId = playlistId,
                        GenreId = randomGenreId,
                    });
                    await unitOfWork.CompleteAsync();
                }

                // If the random Song does not exist in the Playlist<>Song table => Add it.
                if (!this.unitOfWork.PlaylistsSongs.All().Where(x => x.PlaylistId == playlistId).Any(b => b.SongId == randomSongId))
                {
                    await this.unitOfWork.PlaylistsSongs.AddAsync(new PlaylistSong
                    {
                        PlaylistId = playlistId,
                        SongId = randomSongId,
                    });
                    await this.unitOfWork.CompleteAsync();

                    durationDubbedCurrent += randomSong.Duration;
                    listArtistsIdsAlreadyUsed.Add(randomSong.ArtistId);
                }
            }

            Playlist playlistToEdit = await this.unitOfWork.Playlists.All().FirstAsync(x => x.Id == playlistId);

            // If the playlist does not have any Description => add a "default" one.
            if (playlistToEdit.Description == null)
            {
                int countSongsInPlaylist = this.unitOfWork.PlaylistsSongs.All().Where(x => x.PlaylistId == playlistId).Count();
                playlistToEdit.Description = $"'{playlistToEdit.Name}' consists of {countSongsInPlaylist} songs that will make your journey from point A to point B sound like {playlistToEdit.DurationTravel / 60} minutes in Heaven...";
                await this.unitOfWork.CompleteAsync();
            }

            return true;
        }

        public async Task<bool> UpdatePlaylistDurationTravelAsync(Guid id, int durationTravel)
        {
            this.validator.ValidatePlaylistId(id);
            this.validator.ValidateIfPlaylistIsDeleted(id);

            this.validator.ValidateDurationTravel(durationTravel);

            var playlist = await this.unitOfWork.Playlists.All().FirstAsync(x => x.Id == id);

            playlist.DurationTravel = durationTravel;
            await this.unitOfWork.CompleteAsync();
            return true;
        }

        private async Task<string> GatherDataAsync(string url)
        {
            string result = string.Empty;

            var client = new HttpClient();

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    result = await response.Content.ReadAsStringAsync();
                    break;
                }
                catch (Exception)
                {
                    Thread.Sleep(500);
                }
            }

            return result;
        }

        private async Task<List<double>> GetLocationByQueryAsync(string query)
        {
            string apiKey = $"Ah23AFfxih6bgoSaVF8nmoI_GVIKvpR4Fah58v4rMWAC7aZeOGkjIqANdSR1LT-q";
            string queryFixed = query.Trim().Replace(" ", "%20").ToString();

            string url = $"http://dev.virtualearth.net/REST/v1/Locations/{queryFixed}?o=json&key={apiKey}";

            string result = await GatherDataAsync(url);

            if (!string.IsNullOrEmpty(result))
            {
                //LocationRoot rootLocation = JsonConvert.DeserializeObject<LocationRoot>(result);
                //LocationPoint locationPoint = rootLocation.resourceSets.SelectMany(x => x.resources.Select(x => x.point)).FirstOrDefault();
                //if (locationPoint != null && locationPoint.coordinates != null)
                //{
                //    return locationPoint.coordinates;
                //}
            }
            return null;
        }

        private async Task<int> GetDurationByCoordinatesAsync(List<double> coordinatesFirstPoint, List<double> coordinatesSecondPoint)
        {
            string apiKey = $"Ah23AFfxih6bgoSaVF8nmoI_GVIKvpR4Fah58v4rMWAC7aZeOGkjIqANdSR1LT-q";
            string url = $"https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?origins={coordinatesFirstPoint[0]},{coordinatesFirstPoint[1]}&destinations={coordinatesSecondPoint[0]},{coordinatesSecondPoint[1]}&travelMode=driving&key={apiKey}";

            string result = await this.GatherDataAsync(url);

            if (!string.IsNullOrEmpty(result))
            {
                //DistanceRoot rootLocation = JsonConvert.DeserializeObject<DistanceRoot>(result);
                //int duration = Convert.ToInt32(Math.Round(rootLocation.resourceSets.SelectMany(x => x.resources.SelectMany(x => x.results.Select(x => x.travelDuration))).First()));
                //return duration;
            }
            return -1;
        }
    }
}
