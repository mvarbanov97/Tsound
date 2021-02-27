using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TSound.Data;
using TSound.Data.Models;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;

namespace TSound.Services.Extensions
{
    public static class ServiceExtensionMethods
    {
        public static async Task<string> GetJsonStreamFromUrlAsync(this IService service, string url)
        {
            var client = new HttpClient();

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public static async Task<User> ValidateAPIKeyAsync(this IService service, Guid? apiKey, IUnitOfWork unitOfWork)
        {
            if (apiKey is null)
                throw new ArgumentException("Invalid API Key.");

            User userWithThisApiKey = await unitOfWork.Users.All().FirstOrDefaultAsync(x => x.ApiKey == apiKey);

            if (userWithThisApiKey == null || userWithThisApiKey.IsDeleted)
                throw new ArgumentException("Invalid API Key.");

            if (userWithThisApiKey.IsBanned)
                throw new ArgumentException("This User is temporarily Banned.");

            return userWithThisApiKey;
        }

        public static async Task CheckIfUserIsAdminByApiKey(this IService service, Guid? apiKey, IUnitOfWork unitOfWork)
        {
            var userWithKey = await service.ValidateAPIKeyAsync(apiKey, unitOfWork);

            if (!userWithKey.IsAdmin)
                throw new ArgumentException("Only an Admin can trigger an update in the database.");
        }
    }
}
