using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Plugin.Spotify.WebApi.SpotifyModels
{
    public class FeaturedPlaylists : PagedPlaylists
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
