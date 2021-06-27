using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TSound.Data.Models.SpotifyDomainModels;

namespace TSound.Data.Models
{
    public class Track
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("duration_ms")]
        public int DurationMs { get; set; }

        [JsonProperty("explicit")]
        public bool Explicit { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("popularity")]
        public int Popularity { get; set; }

        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [Required]
        [JsonProperty("album")]
        public Album Album { get; set; }
        public Guid AlbumId { get; set; }

        [Required]
        [JsonProperty("artist")]
        public Artist Artist { get; set; }
        public Guid ArtistId { get; set; }

        public string SpotifyCategoryId { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }

        public ICollection<PlaylistTrack> Playlists { get; set; } = new HashSet<PlaylistTrack>();
    }
}
