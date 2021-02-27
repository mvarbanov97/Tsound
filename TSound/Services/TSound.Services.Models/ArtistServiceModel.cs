using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Services.Models
{
    public class ArtistServiceModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ArtistPageURL { get; set; }

        public string PictureURL { get; set; }

        public int AlbumCount { get; set; }

        public int FanCount { get; set; }

        public string SongListURL { get; set; }
    }
}
