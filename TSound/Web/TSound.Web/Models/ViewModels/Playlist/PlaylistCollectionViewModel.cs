using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels.Playlist
{
    public class PlaylistCollectionViewModel
    {
        public IEnumerable<PlaylistViewModel> CollectionPlaylists { get; set; }
    }
}
