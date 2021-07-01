using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Services.Models.News
{
    public class CollectionNewsResults
    {
        [JsonProperty("articles")]
        public ICollection<NewsServiceModel> Articles { get; set; }
    }
}
