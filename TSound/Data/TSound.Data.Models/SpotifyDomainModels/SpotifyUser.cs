using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models.SpotifyDomainModels
{
    public class SpotifyUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls Url { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }
    }

}
