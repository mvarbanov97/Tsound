using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Services.Contracts;
using TSound.Services.Models;
using TSound.Web.Models.ViewModels.Category;
using TSound.Web.Models.ViewModels.Playlist;
using TSound.Web.Models.ViewModels.User;

namespace TSound.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IPlaylistService playlistService;
        private readonly ICategoryService genreService;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService, IMapper mapper, IPlaylistService playlistService, ICategoryService genreService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.mapper = mapper;
            this.playlistService = playlistService;
            this.genreService = genreService;
        }

        [Authorize]
        public async Task<IActionResult> All(int page = 1)
        {
            IEnumerable<UserServiceModel> collectionUserServiceModels = await userService.GetAllUsersAsync(false, page);
            int pageSizeCount = this.userService.GetPageCountSizing();
            int totalCount = this.userService.GetTotalUsersCount();

            CollectionUserFullViewModel model = new CollectionUserFullViewModel();

            model.Users = collectionUserServiceModels.Select(x => new UserViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateCreated = x.DateCreated,
                DateModified = x.DateModified,
                IsBanned = x.IsBanned,
                IsAdmin = x.IsAdmin,
                IsDeleted = x.IsDeleted,
                ImageUrl = x.ImageUrl,
                PlaylistsCount = x.PlaylistsCount,
            });

            model.Url = "/Users/All";
            model.PageSize = pageSizeCount;
            model.CurrentPage = page;
            model.TotalCount = totalCount;

            return this.View(model);
        }

        public async Task<IActionResult> UserById(Guid id)
        {
            UserServiceModel userServiceModel = await this.userService.GetUserByIdAsync(id);

            UserViewModel model = this.mapper.Map<UserViewModel>(userServiceModel);

            if (userServiceModel.PlaylistsCount != 0)
            {
                IEnumerable<PlaylistServiceModel> playlistsOfUser = await this.playlistService.GetPlaylistsByUserIdAsync(id);

                IEnumerable<Guid> playlistsIdsOfUser = playlistsOfUser.Select(x => x.Id);

                HashSet<CategoryServiceModel> genresPreferred = new HashSet<CategoryServiceModel>();

                foreach (var playlistId in playlistsIdsOfUser)
                {
                    var genresOfPlaylists = await this.genreService.GetCategoryByPlaylistIdAsync(playlistId);

                    foreach (var genre in genresOfPlaylists)
                    {
                        if (!genresPreferred.Any(x => x.Id == genre.Id))
                        {
                            genresPreferred.Add(genre);
                        }
                    }
                }

                model.Playlists = this.mapper.Map<IEnumerable<PlaylistLightViewModel>>(playlistsOfUser);
                model.CategoriesPreferred = this.mapper.Map<IEnumerable<CategoryFullViewModel>>(genresPreferred);
            }

            return this.View(model);
        }
    }
}
