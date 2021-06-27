using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class PlaylistCategory
    {
        public Playlist Playlist { get; set; }

        public Guid PlaylistId { get; set; }

        public Category Category { get; set; }

        public Guid CategoryId { get; set; }
    }
}
