using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class PlaylistSong
    {
        public Playlist Playlist { get; set; }

        public Guid PlaylistId { get; set; }

        public Song Song { get; set; }

        public Guid SongId { get; set; }
    }
}
