using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Services.Contracts;
using TSound.Web.Models.ViewModels.Track;

namespace TSound.Web.Controllers
{
    public class TrackController : Controller
    {
        private readonly ITrackService songService;
        private readonly IMapper mapper;

        public TrackController(
            ITrackService songService,
            IMapper mapper)
        {
            this.songService = songService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All()
        {
            var songs = await this.songService.GetAllTracksAsync();

            var songLightViewModels = this.mapper.Map<IEnumerable<TrackViewModel>>(songs);

            return this.View(songLightViewModels);
        }

        public async Task<IActionResult> Song(Guid id)
        {
            var song = await this.songService.GetTrackByIdAsync(id);

            var model = this.mapper.Map<TrackViewModel>(song);

            return this.View(model);
        }

    }
}
