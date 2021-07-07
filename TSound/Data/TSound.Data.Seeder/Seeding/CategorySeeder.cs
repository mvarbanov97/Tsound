using System;
using System.Threading.Tasks;
using TSound.Services.Contracts;

namespace TSound.Data.Seeder.Seeding
{
    public class CategorySeeder : ISeeder
    {
        private readonly ICategoryService categoryService;

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            await this.categoryService.LoadCategoriesInDb();
        }
    }
}
