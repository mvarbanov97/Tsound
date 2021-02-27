using System;

namespace TSound.Web.Models.ViewModels
{
    public class GenreFullViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PictureURL { get; set; }

        public bool IsSelected { get; set; }
    }
}