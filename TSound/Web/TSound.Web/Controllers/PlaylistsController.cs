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
using TSound.Services.Contracts;
using TSound.Services.External;
using TSound.Services.Models;
using TSound.Web.Models;
using TSound.Web.Models.ViewModels;
using TSound.Web.Models.ViewModels.Playlist;

namespace TSound.Web.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IPlaylistService playlistService;
        private readonly IUserService usersService;
        private readonly IGenreService genreService;
        private readonly Cloudinary cloudinary;
        private readonly IMapper mapper;

        public PlaylistsController(
            UserManager<User> userManager,
            IPlaylistService playlistService,
            IUserService usersService,
            IGenreService genreService,
            IMapper mapper,
            Cloudinary cloudinary)
        {
            this.userManager = userManager;
            this.playlistService = playlistService;
            this.usersService = usersService;
            this.genreService = genreService;
            this.mapper = mapper;
            this.cloudinary = cloudinary;
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
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlaylistCreateFormInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            var userId = user.Id;

            PlaylistServiceModel playlistToCreate = new PlaylistServiceModel
            {
                Name = input.Name,
                Description = input.Description,
                UserId = userId,
            };

            return RedirectToAction("Success", "Home");
            //, new SuccessViewModel
            //  {
            //      urlGeneral = "/breweries/all",
            //      urlItemCreated = $"/breweries/brewery/{breweryAdded.Id}"
            //  });
        }

    }
}
