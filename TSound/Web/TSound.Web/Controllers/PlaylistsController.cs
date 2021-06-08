using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Common;
using TSound.Data.Models;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.External;
using TSound.Services.External.SpotifyAuthorization;
using TSound.Services.Models;
using TSound.Web.ApiControllers;
using TSound.Web.Models;
using TSound.Web.Models.ViewModels;
using TSound.Web.Models.ViewModels.Genre;
using TSound.Web.Models.ViewModels.Playlist;
using static TSound.Data.Models.SpotifyDomainModels.SpotifyPlaylistModel;

namespace TSound.Web.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly SpotifyPlaylistApiController spotifyPlaylistApi;
        private readonly UserManager<User> userManager;
        private readonly IPlaylistService playlistService;
        private readonly IUserService usersService;
        private readonly IGenreService genreService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISpotifyAuthService spotifyAuthService;
        private readonly IApplicationCloudinary applicationCloudinary;

        public PlaylistsController(
            UserManager<User> userManager,
            IPlaylistService playlistService,
            IUserService usersService,
            IGenreService genreService,
            IMapper mapper, SpotifyPlaylistApiController spotifyPlaylistApi, IUnitOfWork unitOfWork, ISpotifyAuthService spotifyAuthService, IUserService userService, IApplicationCloudinary applicationCloudinary)
        {
            this.userManager = userManager;
            this.playlistService = playlistService;
            this.usersService = usersService;
            this.genreService = genreService;
            this.mapper = mapper;
            this.spotifyPlaylistApi = spotifyPlaylistApi;
            this.unitOfWork = unitOfWork;
            this.spotifyAuthService = spotifyAuthService;
            this.applicationCloudinary = applicationCloudinary;
        }

        public async Task<IActionResult> All()
        {
            var collectionPlaylistsServiceModels = await playlistService.GetAllPlaylistsAsync();

            PlaylistCollectionViewModel model = new PlaylistCollectionViewModel();
            model.CollectionPlaylists = collectionPlaylistsServiceModels.Select(x => new PlaylistViewModel
            {
                Id = x.Id,
                Name = x.Name,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                Description = x.Description,
                DurationPlaylist = x.DurationPlaylist,
                DurationTravel = x.DurationTravel,
                Image = x.Image,
                Rank = x.Rank,
                IsUnlisted = x.IsUnlisted,
                IsDeleted = x.IsDeleted,
                UserId = x.UserId,
                UserName = x.UserName,
                UserImage = x.UserImage,
            });

            return this.View(model);
        }

        public async Task<IActionResult> Create()
        {
            var genresServiceModels = await this.genreService.GetAllGenresAsync(false, null);
            var genreViewModels = this.mapper.Map<IEnumerable<GenreFullViewModel>>(genresServiceModels).ToList();

            PlaylistCreateFormInputModel model = new PlaylistCreateFormInputModel
            {
                Genres = genreViewModels,
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
            var userRefreshToken = await this.spotifyAuthService.GetUserRefreshToken(currentUser.Id);

            await this.spotifyAuthService.GenerateRefreshRequestBody(currentUser.Id);

            var userAccessToken = await this.spotifyAuthService.GetUserAccessToken(currentUser.Id);

            var spotifyId = this.unitOfWork.UserLogins.All().Where(x => x.UserId == currentUser.Id).Select(x => x.ProviderKey).FirstOrDefault();
            var spotifyUser = await this.usersService.GetSpotifyUser(spotifyId, userAccessToken);

            var details = new PlaylistDetails { Name = input.Name, Description = input.Description, Public = true};

            PlaylistServiceModel playlistToCreate = new PlaylistServiceModel
            {
                Name = input.Name,
                Description = input.Description,
                UserId = currentUser.Id,
                UserName = currentUser.FirstName + " " + currentUser.LastName,
                UserImage = currentUser.ImageUrl,
                DurationTravel = input.DurationMS,
            };

            // Creates the playlist on Spotify server
            var spotifyPlaylist = await this.spotifyPlaylistApi.CreatePlaylist<SpotifyPlaylist>(spotifyUser.Id, details, userAccessToken);
            playlistToCreate.SpotifyId = spotifyPlaylist.Id;

            // Save the playlist into the Db
            var playlistCreated = await this.playlistService.CreatePlaylistAsync(playlistToCreate);

            // Upload Playlist Image to Cloudinary
            await this.applicationCloudinary.UploadImageAsync(
                input.ImageFile,
                string.Format(GlobalConstants.CloudinaryPlaylistPictureName, playlistCreated.Id));
            
            var genresIdsChosenByUser = input.Genres.Where(x => x.IsSelected == true).Select(y => y.SpotifyId);

            // Generate algorithm
            var trackUris = await playlistService.GeneratePlaylistAsync(playlistCreated.Id, playlistToCreate.DurationTravel, genresIdsChosenByUser, userAccessToken);

            await this.spotifyPlaylistApi.AddItemsToPlaylist(spotifyPlaylist.Id, trackUris, null, userAccessToken);

            return RedirectToAction("Success", "Home", new SuccessViewModel
            {
                urlGeneral = "/playlists/all",
                urlItemCreated = $"/Playlists/PlaylistById/{playlistCreated.Id}"
            });
        }

    }
}
