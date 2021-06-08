using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }

        public string SpotifyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string Image { get; set; }

        public int DurationTravel { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsUnlisted { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<PlaylistSong> Songs { get; set; } = new HashSet<PlaylistSong>();
        
        public ICollection<PlaylistGenre> Genres { get; set; } = new HashSet<PlaylistGenre>();
    }
}
