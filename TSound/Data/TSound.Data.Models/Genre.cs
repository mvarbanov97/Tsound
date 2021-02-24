using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TSound.Data.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [JsonProperty("id")]
        public string DeezerId { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string PictureURL { get; set; }

        public int SongCount { get; set; }

        public ICollection<Song> Songs { get; set; } = new HashSet<Song>();

        public ICollection<PlaylistGenre> Playlists { get; set; } = new HashSet<PlaylistGenre>();
    }
}
