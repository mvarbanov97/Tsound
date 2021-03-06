﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Web.Models.ViewModels.Category;
using TSound.Web.Models.ViewModels.Track;

namespace TSound.Web.Models.ViewModels.Playlist
{
    public class PlaylistViewModel
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

        public int GenresCount { get; set; }

        public int SongsCount { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }

        public IEnumerable<TrackLightViewModel> SongsTop3 { get; set; }

        public IEnumerable<CategoryFullViewModel> Genres { get; set; }

    }
}
