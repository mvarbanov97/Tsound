using Newtonsoft.Json;

namespace TSound.Services.Models.News
{
    public class NewsServiceModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("source")]
        public NewsSource Publisher { get; set; }

        [JsonProperty("description")]
        public string Content { get; set; }

        [JsonProperty("url")]
        public string Link { get; set; }

        [JsonProperty("urlToImage")]
        public string ImageUrl { get; set; }
    }
}
