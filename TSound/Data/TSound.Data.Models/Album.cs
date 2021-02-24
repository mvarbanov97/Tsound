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
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("tracklist")]
        public string SonglistUrl { get; set; }

        [JsonProperty("artist")]
        public Artist Artist { get; set; }
        public Guid ArtistId { get; set; }

        [JsonProperty("contributors")]
        public IList<Artist> Contributors { get; set; }

        [Required]
        [JsonProperty("tracks")]
        public IList<Song> Songs { get; set; }

        public int SongCount { get; set; }

    }
}
