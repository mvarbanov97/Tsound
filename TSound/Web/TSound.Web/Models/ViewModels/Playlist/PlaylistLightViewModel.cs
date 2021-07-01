using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Web.Models.Contracts;

namespace TSound.Web.Models.ViewModels.Playlist
{
    public class PlaylistLightViewModel : IDeletable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public string Image { get; set; }

        public double Rank { get; set; }

        public int DurationPlaylist { get; set; }

        public int DurationTravel { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsUnlisted { get; set; }

        public int CategoriesCount { get; set; }

        public int SongsCount { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }

        public string NameController { get; set; }

        public bool IsToBeDeletedByAdmin { get; set; }

        public int CurrentPage { get; set; }
    }
}
