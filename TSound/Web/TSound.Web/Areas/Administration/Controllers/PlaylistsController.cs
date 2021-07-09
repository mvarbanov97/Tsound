using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Services.Contracts;
using TSound.Web.Models.ViewModels.Playlist;

namespace TSound.Web.Areas.Administration.Controllers
{
    public class PlaylistsController : AdminController
    {
        private readonly IPlaylistService playlistService;
        private readonly IMapper mapper;

        public PlaylistsController(
            IPlaylistService playlistService,
            IMapper mapper)
        {
            this.playlistService = playlistService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> ManagePlaylists(int page = 1)
        {
            int pageCountSize = 12;

            var collectionPlaylistsServiceModels = await playlistService.GetAllPlaylistsAsync(false, null, true);

            int totalCount = collectionPlaylistsServiceModels.Count();

            var collectionPlaylistsServiceModelsByPage = collectionPlaylistsServiceModels
                .Skip((page - 1) * pageCountSize)
                .Take(pageCountSize);

            var collectionPlaylistsViewModel = this.mapper.Map<IEnumerable<PlaylistLightViewModel>>(collectionPlaylistsServiceModels);

            var model = new AllPlaylistsViewModel
            {
                CollectionPlaylists = collectionPlaylistsViewModel,
                PageSize = pageCountSize,
                CurrentPage = page,
                TotalCount = totalCount,
                Url = "/Administration/Playlists/ManagePlaylists"
            };

            return await Task.Run(() => View(model));
        }

        public async Task<IActionResult> Remove(Guid id, int page = 1)
        {
            await this.playlistService.DeletePlaylistAsync(id, false, null);

            return Redirect($"/Administration/Playlists/ManagePlaylists?page={page}");
        }
    }
}
