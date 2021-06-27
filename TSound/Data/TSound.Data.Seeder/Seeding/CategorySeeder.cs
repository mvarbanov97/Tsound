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
        private GenreService genreService;
        private UnitOfWork.UnitOfWork unitOfWork;
        private IMapper mapper;
        private HttpClient http = new HttpClient();
        private IConfiguration configuration;
        private IAccountsService accountsService;

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
            this.genreService = new GenreService(this.unitOfWork, this.mapper, this.http, this.configuration, this.accountsService);
            await this.genreService.LoadGenresInDbAsync();
        }
    }
}
