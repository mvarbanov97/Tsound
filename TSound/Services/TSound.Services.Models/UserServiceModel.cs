using System;

namespace TSound.Services.Models
{
    public class UserServiceModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string ImageUrl { get; set; }

        public Guid ApiKey { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

        public bool IsAdmin { get; set; }

        public int PlaylistsCount { get; set; }
    }
}
