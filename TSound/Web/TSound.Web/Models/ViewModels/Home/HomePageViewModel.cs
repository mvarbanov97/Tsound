using System.Collections.Generic;
using TSound.Web.Models.ViewModels.Playlist;
using TSound.Web.Models.ViewModels.Track;

namespace TSound.Web.Models.ViewModels.Home
{
    public class HomePageViewModel
    {
        public IEnumerable<PlaylistLightViewModel> Top3Playlists { get; set; }
        public IEnumerable<TrackLightViewModel> Top3Songs { get; set; }

        public IEnumerable<NewsViewModel> Top3News { get; set; }
    }
}
