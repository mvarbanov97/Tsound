using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TSound.Data.Models
{
    public class Album
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [JsonProperty("album_type")]
        public string AlbumType { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("external_urls:spotify")]
        public string ExternalUrls { get; set; }

        [JsonProperty("total_tracks")]
        public long TotalTracks { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [Required]
        [JsonProperty("tracks")]
        public ICollection<Track> Tracks { get; set; } = new HashSet<Track>();

        [JsonProperty("artists")]
        public ICollection<AlbumArtist> Artists { get; set; } = new HashSet<AlbumArtist>();
    }
}
