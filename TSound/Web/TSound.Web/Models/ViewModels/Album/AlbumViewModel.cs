using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels.Album
{
    public class AlbumViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SonglistUrl { get; set; }

        public string Artist { get; set; }
        public Guid ArtistId { get; set; }
    }
}
