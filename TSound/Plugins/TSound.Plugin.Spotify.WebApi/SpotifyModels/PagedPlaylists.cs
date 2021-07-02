using System;
using System.Collections.Generic;
using System.Text;
using static TSound.Plugin.Spotify.WebApi.SpotifyModels.SpotifyPlaylistModel;

namespace TSound.Plugin.Spotify.WebApi.SpotifyModels
{
    public class PagedPlaylists : Paged<PlaylistSimplified>
    {
    }
}
