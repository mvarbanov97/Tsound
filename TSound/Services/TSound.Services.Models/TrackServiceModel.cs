using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Services.Models
{
    public class TrackServiceModel
    {
        public Guid Id { get; set; }

        public string  SpotifyId { get; set; }

        public string Name { get; set; }

        public int DurationMs { get; set; }

        public int Popularity { get; set; }

        public string PreviewUrl { get; set; }

        public string Uri { get; set; }

        public string Album { get; set; }

        public Guid AlbumId { get; set; }

        public string Artist { get; set; }
        public Guid ArtistId { get; set; }

        public string Genre { get; set; }
        public Guid GenreId { get; set; }
    }
}
