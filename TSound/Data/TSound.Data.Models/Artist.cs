using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TSound.Data.Models.SpotifyDomainModels;

namespace TSound.Data.Models
{
    public class Artist
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls ExternalUrls { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public int? SongCount { get; set; }

        public int? Age { get; set; }

        public DateTime? BirthDate { get; set; }

        public ICollection<Track> Tracks { get; set; } = new HashSet<Track>();

        public ICollection<AlbumArtist> Albums { get; set; } = new HashSet<AlbumArtist>();


    }
}
