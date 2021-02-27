using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Services.Contracts;
using TSound.Web.Models.ViewModels.Song;

namespace TSound.Web.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongService songService;
        private readonly IMapper mapper;

        public SongsController(
            ISongService songService,
            IMapper mapper)
        {
            this.songService = songService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All()
        {
            var songs = await this.songService.GetAllSongsAsync();

            var songLightViewModels = this.mapper.Map<IEnumerable<SongViewModel>>(songs);

            return this.View(songLightViewModels);
        }

        public async Task<IActionResult> Song(Guid id)
        {
            var song = await this.songService.GetSongByIdAsync(id);

            var model = this.mapper.Map<SongViewModel>(song);

            return this.View(model);
        }

    }
}
