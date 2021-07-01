using System;

namespace TSound.Services.Models
{
    public class CategoryServiceModel
    {
        public Guid Id { get; set; }

        public string SpotifyId { get; set; }

        public string Name { get; set; }

        public string PictureURL { get; set; }

        public string ImageUrl { get; set; }
    }
}
