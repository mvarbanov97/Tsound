using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TSound.Data.Seeding
{
    public class TSoundDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(TSoundDbContextSeeder));

            var seeders = new List<ISeeder>
            {
                new RolesSeeder()
            };

            foreach (var seeder in seeders)
            {
                logger.LogInformation($"{seeder.GetType().Name} started!");

                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();

                logger.LogInformation($"{seeder.GetType().Name} completed!");
            }
        }
    }
}
