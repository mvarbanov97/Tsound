using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Common;
using TSound.Data.Models;
using TSound.Plugin.Spotify.WebApi.Authorization;
using TSound.Plugin.Spotify.WebApi.Contracts;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;
using TSound.Services.Contracts;
using TSound.Services.External;
using TSound.Services.Models;
using TSound.Web.Models;
using TSound.Web.Models.ViewModels.Category;
using TSound.Web.Models.ViewModels.Playlist;
using TSound.Web.Models.ViewModels.Track;
using static TSound.Plugin.Spotify.WebApi.SpotifyModels.SpotifyPlaylistModel;

namespace TSound.Web.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IPlaylistApi spotifyPlaylistApi;
        private readonly IUserProfileApi userProfileApi;
        private readonly IPlaylistService playlistService;
        private readonly IUserService usersService;
        private readonly ICategoryService categoryService;
        private readonly ITrackService trackService;
        private readonly IMapper mapper;
        private readonly IUserAccountsService userAccountsService;
        private readonly IApplicationCloudinary applicationCloudinary;

        public PlaylistsController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IPlaylistApi spotifyPlaylistApi,
            IUserProfileApi userProfileApi,
            IPlaylistService playlistService,
            IUserService usersService,
            ICategoryService categoryService,
            ITrackService trackService,
            IMapper mapper,
            IUserAccountsService userAccountsService,
            IApplicationCloudinary applicationCloudinary)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.spotifyPlaylistApi = spotifyPlaylistApi;
            this.userProfileApi = userProfileApi;
            this.playlistService = playlistService;
            this.usersService = usersService;
            this.categoryService = categoryService;
            this.trackService = trackService;
            this.mapper = mapper;
            this.userAccountsService = userAccountsService;
            this.applicationCloudinary = applicationCloudinary;
        }

        public async Task<IActionResult> All(int page = 1)
        {
            int pageCountSize = 12;

            IEnumerable<PlaylistServiceModel> collectionPlaylistsServiceModels = await playlistService.GetAllPlaylistsAsync(false, null, false);

            int totalCount = collectionPlaylistsServiceModels.Count();

            var collectionPlaylistsServiceModelsByPage = collectionPlaylistsServiceModels
                .OrderByDescending(x => x.Rank)
                .Skip((page - 1) * pageCountSize)
                .Take(pageCountSize);

            AllPlaylistsViewModel model = new AllPlaylistsViewModel();
            model.CollectionPlaylists = this.mapper.Map<IEnumerable<PlaylistLightViewModel>>(collectionPlaylistsServiceModelsByPage);

            model.Url = "/Playlists/All";
            model.PageSize = pageCountSize;
            model.CurrentPage = page;
            model.TotalCount = totalCount;

            var catgoryServiceModels = await categoryService.GetAllCategoriesAsync(false, null);
            model.Categories = this.mapper.Map<IEnumerable<CategoryFullViewModel>>(catgoryServiceModels).ToList();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> All(AllPlaylistsViewModel input)
        {
            IEnumerable<PlaylistServiceModel> playlists;

            if (input.NameToSearchForFilter != null)
            {
                // Filter by name
                playlists = await this.playlistService.GetPlaylistsByContainsSubstring(input.NameToSearchForFilter);
            }
            else
            {
                playlists = await playlistService.GetAllPlaylistsAsync(false, null, false);

                // Filter By Duration
                if (input.DurationMinHoursFilter != 0 || input.DurationMaxHoursFilter != 0)
                {
                    playlists = playlistService.FilterByRange(playlists, "duration", input.DurationMinHoursFilter, input.DurationMaxHoursFilter);
                }

                // TODO: Add Filter by Rank

                // Filter By Category
                if (input.Categories != null && input.Categories.Count() != 0)
                {
                    var genresIdsChosenByUser = input.Categories.Where(x => x.IsSelected == true).Select(y => y.Id);
                    playlists = playlistService.FilterByCategory(playlists, genresIdsChosenByUser);
                }

                // Sort
                string sortMethod = input.SortMethod.ToString().ToLowerInvariant();
                string sortOrder = input.SortOrder.ToString().ToLowerInvariant();

                if (sortMethod == "sort")
                {
                    sortMethod = "rank";
                }
                if (sortOrder == "order")
                {
                    sortOrder = "desc";
                }

                playlists = playlistService.Sort(playlists, sortMethod, sortOrder);
            }

            AllPlaylistsViewModel model = new AllPlaylistsViewModel();
            model.CollectionPlaylists = this.mapper.Map<IEnumerable<PlaylistLightViewModel>>(playlists);

            var genresServiceModels = await this.categoryService.GetAllCategoriesAsync(false, null);
            model.Categories = this.mapper.Map<IEnumerable<CategoryFullViewModel>>(genresServiceModels).ToList();

            model.CurrentPage = 1;
            model.Url = "/Playlists/All";

            return this.View(model);
        }

        public async Task<IActionResult> MyPlaylists(string email)
        {
            var user = await this.usersService.GetUserByEmailAsync(email);
            var collectionPlaylistsByUserId = await playlistService.GetPlaylistsByUserIdAsync(user.Id);
            AllPlaylistsViewModel model = new AllPlaylistsViewModel();
            model.CollectionPlaylists = this.mapper.Map<IEnumerable<PlaylistLightViewModel>>(collectionPlaylistsByUserId);

            return this.View(model);
        }

        public async Task<IActionResult> PlaylistById(Guid id)
        {
            var playlistServiceModel = await playlistService.GetPlaylistByIdAsync(id);

            var categories = await this.categoryService.GetCategoryByPlaylistIdAsync(playlistServiceModel.Id);
            var tracks = await this.trackService.GetTracksByPlaylistIdAsync(id);

            var playlistAll = await playlistService.GetAllPlaylistsAsync();

            var model = this.mapper.Map<PlaylistViewModel>(playlistServiceModel);

            model.Genres = this.mapper.Map<IEnumerable<CategoryFullViewModel>>(categories);
            model.SongsTop3 = this.mapper.Map<IEnumerable<TrackLightViewModel>>(tracks.OrderBy(s => s.Popularity).Take(3));

            return this.View(model);
        }

        public async Task<IActionResult> Create()
        {
            var categoriesServiceModels = await this.categoryService.GetAllCategoriesAsync(false, null);
            var categoryViewModel = this.mapper.Map<IEnumerable<CategoryFullViewModel>>(categoriesServiceModels).ToList();

            PlaylistCreateFormInputModel model = new PlaylistCreateFormInputModel
            {
                Categories = categoryViewModel,
            };

            return await Task.Run(() => View(model));
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlaylistCreateFormInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            // Check if user has logged in with External Provider (Spotify)
            var logins = await this.signInManager.UserManager.GetLoginsAsync(currentUser);
            // if (logins == null)
                // Crate playlist with tracks that are seeded in the Db

            // Access token expires every 1 hour so we need Refresh token in order to enquire new one
            var refreshToken = await this.userAccountsService.GetRefreshTokenFromDb(currentUser.Id);
            var token = await this.userAccountsService.RefreshUserAccessToken(refreshToken);

            // var spotifyUser = await this.usersService.GetCurrentUserSpotifyProfile(token.AccessToken);
            var spotifyUser = await this.userProfileApi.GetCurrentUsersProfile(token.AccessToken);

            var details = new PlaylistDetails { Name = input.Name, Description = input.Description, Public = true };

            // Creates the playlist using the Spotify Api
            var spotifyPlaylist = await this.spotifyPlaylistApi.CreatePlaylist<SpotifyPlaylist>(spotifyUser.Id, details, token.AccessToken);

            // Fetch random tracks from Spotify Playlist API with user selected categories
            var categoryIdsChoosenByUser = input.Categories.Where(x => x.IsSelected == true).Select(y => y.SpotifyId);
            var tracks = await this.playlistService.GenerateTracksForPlaylistAsync(input.DurationMS, categoryIdsChoosenByUser, token.AccessToken);

            await this.trackService.AddTracksToDbAsync(tracks);

            // Add tracks to Spotify Playlist   
            var trackUris = tracks.Select(x => x.Track.Uri).ToArray();
            await this.spotifyPlaylistApi.AddItemsToPlaylist(spotifyPlaylist.Id, trackUris, null, token.AccessToken);

            // Get the auto generated Cover Image
            var image = await this.spotifyPlaylistApi.GetPlaylistCoverImage(spotifyPlaylist.Id, token.AccessToken);

            PlaylistServiceModel playlistToCreate = new PlaylistServiceModel
            {
                Name = input.Name,
                Description = input.Description,
                UserId = currentUser.Id,
                UserName = currentUser.FirstName + " " + currentUser.LastName,
                UserImageUrl = currentUser.ImageUrl,
                DurationTravel = input.DurationMS,
                SpotifyId = spotifyPlaylist.Id,
                Image = image[0].Url
            };

            // Save the playlist into the Db
            var playlistCreated = await this.playlistService.CreatePlaylistAsync(playlistToCreate);
            await this.playlistService.AddTracksToPlaylist(playlistCreated.Id, tracks);

            // Upload Playlist Image to Cloudinary
            if (input.ImageFile != null)
                await this.applicationCloudinary.UploadImageAsync(
                    input.ImageFile,
                    string.Format(GlobalConstants.CloudinaryPlaylistPictureName, playlistCreated.Id));

            await this.playlistService.AddCategoriesToPlaylist(playlistCreated.Id, categoryIdsChoosenByUser);

            return RedirectToAction("Success", "Home", new SuccessViewModel
            {
                urlGeneral = "/playlists/all",
                urlItemCreated = $"/Playlists/PlaylistById/{playlistCreated.Id}"
            });
        }

        [Authorize]
        public async Task<IActionResult> Update(Guid id)
        {
            PlaylistServiceModel playlistToUpdate = await this.playlistService.GetPlaylistByIdAsync(id);

            var input = this.mapper.Map<PlaylistViewModel>(playlistToUpdate);

            return this.View(input);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(PlaylistViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            PlaylistServiceModel playlistToUpdate = new PlaylistServiceModel
            {
                Id = input.Id,
                Name = input.Name,
                Description = input.Description
            };

            await this.playlistService.UpdatePlaylistAsync(playlistToUpdate);

            return RedirectToAction("Success", "Home", new SuccessViewModel
            {
                urlGeneral = "/Playlists/All",
                urlItemCreated = $"/Playlists/PlaylistById/{input.Id}"
            });
        }
    }
}
