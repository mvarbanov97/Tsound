using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.UnitOfWork;
using TSound.Services;

namespace TSound.Data.Seeder.Seeding
{
    public class GenresSeeder : ISeeder
    {
        private GenreService genreService;
        private UnitOfWork.UnitOfWork unitOfWork;
        private IMapper mapper;

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
            this.genreService = new GenreService(this.unitOfWork, this.mapper);
            await this.genreService.LoadGenresInDbAsync();
        }
    }
}
