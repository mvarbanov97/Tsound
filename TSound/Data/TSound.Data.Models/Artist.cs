using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TSound.Data.Models
{
    public class Artist
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
        [JsonProperty("link")]
        public string ArtistPageURL { get; set; }

        [JsonProperty("picture")]
        public string PictureURL { get; set; }

        [JsonProperty("nb_album")]
        public int AlbumCount { get; set; }

        [JsonProperty("nb_fan")]
        public int FanCount { get; set; }

        [Required]
        [JsonProperty("tracklist")]
        public string SongListURL { get; set; }

        public int? SongCount { get; set; }

        public int? Age { get; set; }

        public DateTime? BirthDate { get; set; }

        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();

        public ICollection<Album> Albums { get; set; } = new HashSet<Album>();

    }
}
