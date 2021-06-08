using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.External;
using TSound.Services.External.SpotifyAuthorization;
using TSound.Services.Models.ApiModels;
using TSound.Web.Models;
using static TSound.Data.Models.SpotifyDomainModels.SpotifyPlaylistModel;

namespace TSound.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<TSound.Data.Models.User> userManager;
        private readonly SignInManager<TSound.Data.Models.User> signInManager;
        private readonly IUserService userService;
        private readonly IGenreService genreService;
        private readonly IPlaylistService playlistService;
        private readonly IAccountsService accountsService;
        private readonly IUserAccountsService userAccountsService;
        private readonly ISpotifyAuthService spotifyAuthService;
        private readonly HttpClient client;
        private readonly IUnitOfWork unitOfWork;


        public HomeController(ILogger<HomeController> logger, UserManager<TSound.Data.Models.User> userManager, SignInManager<TSound.Data.Models.User> signInManager, IUserService userService, IGenreService genreService, IPlaylistService playlistService, IAccountsService accountsService, IUserAccountsService userAccountsService, HttpClient client, IUnitOfWork unitOfWork, ISpotifyAuthService spotifyAuthService)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.genreService = genreService;
            this.playlistService = playlistService;
            this.accountsService = accountsService;
            this.userAccountsService = userAccountsService;
            this.client = client;
            this.unitOfWork = unitOfWork;
            this.spotifyAuthService = spotifyAuthService;
        }

        public async Task<IActionResult> Index()
        {
            await this.genreService.LoadGenresInDbAsync();
            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Success(SuccessViewModel model)
        {
            try
            {
                return this.View(model);
            }
            catch (ArgumentException ex)
            {
                return Redirect($"/Home/Error?errorMessage={ex.Message}");
            }
        }
    }
}
