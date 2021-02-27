﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Services.Models
{
    public class SongServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SongURL { get; set; }

        public int Duration { get; set; }

        public int Rank { get; set; }

        public string PreviewURL { get; set; }

        public string Album { get; set; }

        public Guid AlbumId { get; set; }

        public string Artist { get; set; }
        public Guid ArtistId { get; set; }

        public string Genre { get; set; }
        public Guid GenreId { get; set; }
    }
}
