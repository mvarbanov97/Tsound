using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class PlaylistGenre
    {
        public Playlist Playlist { get; set; }

        public Guid PlaylistId { get; set; }

        public Genre Genre { get; set; }

        public Guid GenreId { get; set; }

    }
}
