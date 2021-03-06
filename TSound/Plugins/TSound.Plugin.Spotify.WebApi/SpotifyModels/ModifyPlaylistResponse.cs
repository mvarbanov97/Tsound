﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Plugin.Spotify.WebApi.SpotifyModels
{
    public class ModifyPlaylistResponse
    {
        /// <summary>
        /// The snapshot_id can be used to identify your playlist version in future requests.
        /// </summary>
        [JsonProperty("snapshot_id")]
        public string SnapshotId { get; set; }
    }
}
