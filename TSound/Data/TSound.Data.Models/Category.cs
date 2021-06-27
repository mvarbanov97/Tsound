using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TSound.Data.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("href")]
        public string Link { get; set; }

        public string ImageUrl { get; set; }

        public int SongCount { get; set; }

        public ICollection<Track> Tracks { get; set; } = new HashSet<Track>();

        public ICollection<PlaylistCategory> Playlists { get; set; } = new HashSet<PlaylistCategory>();
    }
}
