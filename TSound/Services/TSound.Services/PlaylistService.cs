using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.External.Helpers;
using TSound.Services.External.SpotifyAuthorization;
using TSound.Services.Models;
using TSound.Services.Validators;
using static TSound.Data.Models.SpotifyDomainModels.SpotifyPlaylistModel;

namespace TSound.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ServiceValidator validator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IAccountsService accountsService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly HttpClient http;

        public PlaylistService(
            IUnitOfWork unitOfWork,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper,
            HttpClient http,
            IAccountsService accountsService, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.dateTimeProvider = dateTimeProvider;
            this.mapper = mapper;
            this.http = http;
            this.accountsService = accountsService;
            this.userService = userService;
        }

        /// <summary>
        /// An async method that creates a playlist based on the information received in a PlaylistServiceModel that comes as a method parameter.
        /// </summary>
        /// <param name="playlistServiceModel">A PlaylistServiceModel that keeps the information of the playlist to be created.</param>
        /// <returns>A task that represents a PlaylistServiceModel that holds all information on the newly created user, including the new Guid id.</returns>
        public async Task<PlaylistServiceModel> CreatePlaylistAsync(PlaylistServiceModel playlistServiceModel)
        {
            //this.validator.ValidateIfNameIsNullOrEmpty(playlistServiceModel.Name);
            //this.validator.ValidateIfAuthorExistsInDb(playlistServiceModel.UserId);
            //this.validator.ValidateIfAuthorIsDeleted(playlistServiceModel.UserId);

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

        public async Task<string[]> GeneratePlaylistAsync(Guid playlistId, int durationTravel, IEnumerable<string> categoryIdsToUse, string userAccessToken)
        {
            this.ValidatePlaylistId(playlistId);
            this.ValidateDurationTravel(durationTravel);

            Random random = new Random();
            int durationDubbedCurrent = 0;
            var trackUrisToAdd = new string[100];

            // If for some reason we don't receive any Genres by the User - use all available Genres.
            if (categoryIdsToUse == null || categoryIdsToUse.Count() == 0)
            {
                categoryIdsToUse = this.unitOfWork.Genres.All().Select(x => x.SpotifyId);
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

                for (int i = 0; i < randomTracksOfPlaylist.Items.Count(); i++)
                {
                    trackUrisToAdd[i] = randomTracksOfPlaylist.Items[i].Track.Uri;
                    durationDubbedCurrent += randomTracksOfPlaylist.Items[i].Track.DurationMs;
                }
            }
            
            // Remove any emty values in the array, so the POST request to Spotify Playlist Api do not fail
            return trackUrisToAdd.Where(x => !string.IsNullOrEmpty(x)).ToArray();
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

        //protected internal virtual async Task<T> GetModelFromProperty<T>(
        //    Uri uri,
        //    string rootPropertyName,
        //    string accessToken = null)
        //{
        //    var jObject = await GetJObject(uri, accessToken: accessToken);
        //    if (jObject == null) return default;
        //    return jObject[rootPropertyName].ToObject<T>();
        //}

        //protected internal virtual async Task<JObject> GetJObject(Uri uri, string accessToken = null)
        //{
        //    string json = await this.http.Get
        //    (
        //        uri,
        //        new AuthenticationHeaderValue("Bearer", accessToken ?? (await this.accountsService.GetAccessToken()))
        //    );

        //    // Todo #25 return 204 no content result 
        //    if (string.IsNullOrEmpty(json)) return null;

        //    JObject deserialized = JsonConvert.DeserializeObject(json) as JObject;
        //    if (deserialized == null)
        //        throw new InvalidOperationException($"Failed to deserialize response as JSON. Response = {json.Substring(0, Math.Min(json.Length, 256))}");

        //    return deserialized;
        //}


        //public async Task<T> CreateSpotifyPlaylist<T>(
        //    string userId,
        //    PlaylistDetails details,
        //    string accessToken = null)
        //{
        //    if (string.IsNullOrWhiteSpace(userId)) throw new
        //            ArgumentException("A valid Spotify user id must be specified.");

        //    if (details == null || string.IsNullOrWhiteSpace(details.Name)) throw new
        //            ArgumentException("A PlaylistDetails object param with new playlist name must be provided.");

        //    this.http.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", accessToken ?? (await this.accountsService.GetAccessToken()));

        //    StringContent content = null;


        //    content = new StringContent(JsonConvert.SerializeObject(details));
        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


        //    HttpResponseMessage response = null;
        //    var builder = new UriBuilder($"https://api.spotify.com/v1/playlists/{userId}/tracks");

        //    response = await this.http.PostAsync(builder.Uri.ToString(), content);

        //    var spotifyResponse = new SpotifyResponse<T>
        //    {
        //        StatusCode = response.StatusCode,
        //        ReasonPhrase = response.ReasonPhrase
        //    };

        //    if (response.Content != null)
        //    {
        //        string json = await response.Content.ReadAsStringAsync();
        //        if (!string.IsNullOrEmpty(json)) spotifyResponse.Data = JsonConvert.DeserializeObject<T>(json);
        //    }

        //    var userSpotifyId = await this.userService.GetUserSpotifyId(userId);
        //    return default(T); 
        //}
    }
}
