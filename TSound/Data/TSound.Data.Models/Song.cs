using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TSound.Data.Models
{
    public class Song
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
        [JsonProperty("link")]
        public string SongURL { get; set; }

        [Required]
        [JsonProperty("duration")]
        public double Duration { get; set; }

        [Required]
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("release_date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [JsonProperty("preview")]
        public string PreviewURL { get; set; }

        [Required]
        [JsonProperty("album")]
        public Album Album { get; set; }
        public Guid AlbumId { get; set; }

        [Required]
        [JsonProperty("artist")]
        public Artist Artist { get; set; }
        public Guid ArtistId { get; set; }

        public Genre Genre { get; set; }
        public Guid GenreId { get; set; }

    }
}
