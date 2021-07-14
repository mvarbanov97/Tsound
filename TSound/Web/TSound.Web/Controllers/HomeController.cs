using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TSound.Services.Contracts;
using TSound.Web.Models;
using TSound.Web.Models.ViewModels;
using TSound.Web.Models.ViewModels.Home;
using TSound.Web.Models.ViewModels.Playlist;
using TSound.Web.Models.ViewModels.Track;
using TSound.Web.Models.ViewModels.User;

namespace TSound.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlaylistService playlistService;
        private readonly ITrackService songService;
        private readonly IHomeService homeService;
        private readonly IUserService userService;
        private readonly IMapper mapper;


        public HomeController(
            IPlaylistService playlistService,
            ITrackService songService,
            IHomeService homeService,
            IUserService userService,
            IMapper mapper)
        {
            this.playlistService = playlistService;
            this.songService = songService;
            this.homeService = homeService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var top3Playlists = await this.playlistService.Get3RandomPlaylists();
            var top3Tracks = await this.songService.GetTop3TracksAsync();
            var top3News = await this.homeService.GetMusicNews();

            var playlistViewModels = this.mapper.Map<IEnumerable<PlaylistLightViewModel>>(top3Playlists);
            var songViewModels = this.mapper.Map<IEnumerable<TrackLightViewModel>>(top3Tracks);
            var newsViewModels = this.mapper.Map<IEnumerable<NewsViewModel>>(top3News);

            var model = new HomePageViewModel
            {
                Top3Playlists = playlistViewModels,
                Top3Songs = songViewModels,
                Top3News = newsViewModels
            };

            return View(model);
        }

        public async Task<IActionResult> Privacy()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var users = await userService.GetAllUsersAsync();
            var adminsOnly = users.Where(x => x.IsAdmin == true);
            CollectionUserFullViewModel model = new CollectionUserFullViewModel();
            model.Users = mapper.Map<IEnumerable<UserViewModel>>(adminsOnly);
            return this.View(model);
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

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
