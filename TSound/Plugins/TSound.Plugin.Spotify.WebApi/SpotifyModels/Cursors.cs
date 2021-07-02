using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Plugin.Spotify.WebApi.SpotifyModels
{
    public class Cursors
    {
        /// <summary>
        /// The cursor to use as key to find the next page of items.
        /// </summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>
        /// The cursor to use as key to find the previous page of items.
        /// </summary>
        [JsonProperty("before")]
        public string Before { get; set; }
    }
}
