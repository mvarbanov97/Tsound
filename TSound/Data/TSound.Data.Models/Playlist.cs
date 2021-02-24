using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models
{
    public class Playlist
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        //public int Duration { get; set; }

        //public int Rank { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<PlaylistSong> Songs { get; set; } = new HashSet<PlaylistSong>();
        
        public ICollection<PlaylistGenre> Genres { get; set; } = new HashSet<PlaylistGenre>();
    }
}
