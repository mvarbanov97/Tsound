using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Web.Models.Contracts;
using TSound.Web.Models.ViewModels.Category;
using TSound.Web.Models.ViewModels.Playlist;

namespace TSound.Web.Models.ViewModels.User
{
    public class UserViewModel : IDeletable
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public string ImageUrl { get; set; }

        public Guid ApiKey { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

        public bool IsAdmin { get; set; }

        public int PlaylistsCount { get; set; }

        public IEnumerable<PlaylistLightViewModel> Playlists { get; set; }

        public IEnumerable<CategoryFullViewModel> CategoriesPreferred { get; set; }
        public string Name { get; set; }
        public string NameController { get; set; }
        public bool IsToBeDeletedByAdmin { get; set; }
        public int CurrentPage { get; set; }
    }
}
