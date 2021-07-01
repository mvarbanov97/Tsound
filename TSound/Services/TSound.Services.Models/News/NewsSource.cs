using Newtonsoft.Json;

namespace TSound.Services.Models.News
{
    public class NewsSource
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
