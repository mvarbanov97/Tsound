using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Data.Models.SpotifyDomainModels
{
    public class FeaturedPlaylists : PagedPlaylists
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
