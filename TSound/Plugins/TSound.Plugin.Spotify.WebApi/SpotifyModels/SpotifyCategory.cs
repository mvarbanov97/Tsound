using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Plugin.Spotify.WebApi.SpotifyModels
{
    public partial class SpotifyCategory
    {
        /// <summary>
        /// A link to the Web API endpoint returning full details of the category.
        /// </summary>
        [JsonProperty("href")]
        public string Href { get; set; }

        /// <summary>
        /// The category icon, in various sizes.
        /// </summary>
        [JsonProperty("icons")]
        public Image[] Icons { get; set; }

        /// <summary>
        /// The Spotify category ID of the category.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
