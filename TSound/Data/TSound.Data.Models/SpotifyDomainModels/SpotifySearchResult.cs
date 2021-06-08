using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static TSound.Data.Models.SpotifyDomainModels.SpotifyPlaylistModel;

namespace TSound.Data.Models.SpotifyDomainModels
{
    /// <summary>
    /// Search Result.
    /// </summary>
    /// <remarks> https://developer.spotify.com/documentation/web-api/reference/search/search/ </remarks>
    public partial class SpotifySearchResult
    {
        [JsonProperty("artists")]
        public ArtistsSearchResult Artists { get; set; }

        [JsonProperty("albums")]
        public AlbumsSearchResult Albums { get; set; }

        [JsonProperty("tracks")]
        public TracksSearchResult Tracks { get; set; }

        [JsonProperty("playlists")]
        public PlaylistsSearchResult Playlists { get; set; }
    }

    /// <summary>
    /// Artists Search Result.
    /// </summary>
    public partial class ArtistsSearchResult : Paged<SpotifyArtist>
    {
    }

    /// <summary>
    /// Albums Search Result.
    /// </summary>
    public partial class AlbumsSearchResult : Paged<SpotifyAlbum>
    {
    }

    /// <summary>
    /// Tracks Search Result.
    /// </summary>
    public partial class TracksSearchResult : Paged<SpotifyTrack>
    {
    }

    /// <summary>
    /// Playlists Search Result
    /// </summary>
    public partial class PlaylistsSearchResult : Paged<PlaylistSimplified>
    {
    }
}
