using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class AlbumArtist
    {
        public Album Album { get; set; }

        public Guid AlbumId { get; set; }

        public Artist Artist { get; set; }

        public Guid ArtistId { get; set; }
    }
}
