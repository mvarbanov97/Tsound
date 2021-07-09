using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Services.Contracts;
using TSound.Web.Models.ViewModels.User;

namespace TSound.Web.Areas.Administration.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> ManageUsers(int page = 1)
        {
            var collectionUsersServiceModels = await userService.GetAllUsersAsync(true, page);
            var pageSizeCount = this.userService.GetPageCountSizing();
            var totalCount = this.userService.GetTotalUsersCount();

            var model = new CollectionUserFullViewModel
            {
                Users = collectionUsersServiceModels.Select(x => new UserViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateModified,
                    IsAdmin = x.IsAdmin,
                    IsBanned = x.IsBanned,
                    IsDeleted = x.IsDeleted,
                    ImageUrl = x.ImageUrl,
                    PlaylistsCount = x.PlaylistsCount,
                    ApiKey = x.ApiKey
                }),
                PageSize = pageSizeCount,
                CurrentPage = page,
                TotalCount = totalCount,
                Url = "/Administration/Users/ManageUsers"
            };

            return this.View(model);
        }

        public async Task<IActionResult> SwapBanStatus(Guid id, bool isSwappedByAdmin, int page = 1)
        {
            await userService.SwapUserBanStatusByIdAsync(id);

            if (isSwappedByAdmin)
            {
                return Redirect($"/Administration/Users/ManageUsers?page={page}");
            }
            return Redirect($"/Users/All?page={page}");
        }
    }
}
