using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.Models.News;

namespace TSound.Services
{
    public class HomeService : IHomeService
    {
        private const string newsApiKey = "7a72c8b3f54440eda78efb38b7a7e2fc";


        public async Task<IEnumerable<NewsServiceModel>> GetMusicNews()
        {
            var url = $"http://newsapi.org/v2/everything?q=music&language=en&sortBy=publishedAt&apiKey={newsApiKey}";

            try
            {
                var jsonNews = await this.GetJsonStreamFromUrlAsync(url);
                var news = JsonConvert.DeserializeObject<CollectionNewsResults>(jsonNews);

                var newsServiceModel = news.Articles.Take(3).ToList();

                this.TrimNewsContent(newsServiceModel);

                return newsServiceModel;
            }
            catch
            {
                return new List<NewsServiceModel>();
            }
        }

        private void TrimNewsContent(List<NewsServiceModel> news)
        {
            foreach (var item in news)
            {
                if (item.Content.Length > 100)
                {
                    item.Content = item.Content.Substring(0, 100) + "...";
                }
            }
        }
    }
}
