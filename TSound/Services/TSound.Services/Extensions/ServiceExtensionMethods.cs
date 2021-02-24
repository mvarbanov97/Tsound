using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Contracts;

namespace TSound.Services.Extensions
{
    public static class ServiceExtensionMethods
    {
        public static async Task<string> GetJsonStreamFromDeezerAsync(this IService service, string url)
        {
            var client = new HttpClient();

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); 
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

    }
}
