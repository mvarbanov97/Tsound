using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Services.Models
{
    public class AlbumServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SonglistUrl { get; set; }

        public string Artist { get; set; }

        public Guid ArtistId { get; set; }
    }
}
