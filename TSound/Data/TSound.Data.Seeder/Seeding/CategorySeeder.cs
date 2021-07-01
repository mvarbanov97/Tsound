using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using TSound.Services;
using TSound.Services.External.SpotifyAuthorization;

namespace TSound.Data.Seeder.Seeding
{
    public class CategorySeeder : ISeeder
    {
        private CategoryService genreService;
        private UnitOfWork.UnitOfWork unitOfWork;
        private IMapper mapper;
        private HttpClient http = new HttpClient();
        private IConfiguration configuration;

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
            this.genreService = new CategoryService(this.unitOfWork, this.mapper, this.http, this.configuration);
            await this.genreService.LoadCategoriesInDb();
        }
    }
}
