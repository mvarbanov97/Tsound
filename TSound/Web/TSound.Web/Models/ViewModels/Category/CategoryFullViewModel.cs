using System;

namespace TSound.Web.Models.ViewModels.Category
{
    public class CategoryFullViewModel
    {
        public Guid Id { get; set; }

        public string SpotifyId { get; set; }

        public string Name { get; set; }

        public string PictureURL { get; set; }

        public string ImageUrl { get; set; }

        public bool IsSelected { get; set; }
    }
}