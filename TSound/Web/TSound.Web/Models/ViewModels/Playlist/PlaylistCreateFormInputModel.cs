using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels.Playlist
{
    public class PlaylistCreateFormInputModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile ImageFile { get; set; }

        public bool IsTopTracksOptionUsed { get; set; }

        public bool IsTracksFromSameArtistAllowed { get; set; }

    }
}
