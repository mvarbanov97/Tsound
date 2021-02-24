using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models.DeezerModels
{
    public class DeezerPlaylist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tracklist")]
        public string SongListURL { get; set; }

    }
}
