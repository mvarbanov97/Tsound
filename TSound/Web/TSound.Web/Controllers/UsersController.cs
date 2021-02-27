using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Services.Contracts;
using TSound.Services.Models;
using TSound.Web.Models.ViewModels.User;

namespace TSound.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserService userService;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager, IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<UserServiceModel> collectionUserServiceModels = await userService.GetAllUsersAsync();

            UsersCollectionViewModel model = new UsersCollectionViewModel();
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
                Image = x.Image,
            });

            return this.View(model);
        }

    }
}
