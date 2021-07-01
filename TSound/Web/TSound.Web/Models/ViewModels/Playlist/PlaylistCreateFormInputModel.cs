using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TSound.Web.Models.ViewModels.Category;

namespace TSound.Web.Models.ViewModels.Playlist
{
    public class PlaylistCreateFormInputModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile ImageFile { get; set; }

        public bool IsTopTracksOptionEnabled { get; set; }

        public bool IsTracksFromSameArtistEnabled { get; set; }

        public List<CategoryFullViewModel> Categories { get; set; }

        public List<string> CategoryIdsChosenByUser { get; set; }

        public int DurationMS { get; set; }
    }
}
