using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models.DeezerModels
{
    public class DeezerPlaylistData
    {
        [JsonProperty("data")]
        public List<DeezerPlaylist> Playlists { get; set; }

    }
}
