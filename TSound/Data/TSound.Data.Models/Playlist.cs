using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }

        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("collaborative")]
        public bool Collaborative { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string Image { get; set; }

        public int DurationTravel { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsUnlisted { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public int SongsCount { get; set; }

        public ICollection<PlaylistTrack> Tracks { get; set; } = new HashSet<PlaylistTrack>();
        
        public ICollection<PlaylistCategory> Categories { get; set; } = new HashSet<PlaylistCategory>();
    }
}
