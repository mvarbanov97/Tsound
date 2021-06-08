using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Services.Models
{
    public class PlaylistServiceModel
    {
        public Guid Id { get; set; }

        public string SpotifyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public string Image { get; set; }

        public int Rank { get; set; }

        public int DurationPlaylist { get; set; }

        public int DurationTravel { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsUnlisted { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
    }
}
