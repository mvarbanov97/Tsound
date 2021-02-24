using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models.DeezerModels
{
    public class DeezerTrackList
    {
        [JsonProperty("data")]
        public List<Song> Songs { get; set; }

    }
}
