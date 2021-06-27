using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class PlaylistTrack
    {
        public Playlist Playlist { get; set; }

        public Guid PlaylistId { get; set; }

        public Track Track { get; set; }

        public Guid TrackId { get; set; }
    }
}
