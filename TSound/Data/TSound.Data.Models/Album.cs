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
        public string DeezerId { get; set; }

        [Required]
        [JsonProperty("title")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("tracklist")]
        public string SonglistUrl { get; set; }

        [Required]
        [JsonProperty("artist")]
        public Artist Artist { get; set; }
        public Guid ArtistId { get; set; }

        [Required]
        [JsonProperty("tracks")]
        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();

        public int SongCount { get; set; }
    }
}
