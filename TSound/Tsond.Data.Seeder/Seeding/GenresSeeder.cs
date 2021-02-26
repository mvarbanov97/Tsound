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
        private TSoundDbContext dbContext;
        private UnitOfWork.UnitOfWork unitOfWork;

        public GenresSeeder(TSoundDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.unitOfWork = new UnitOfWork.UnitOfWork(this.dbContext);
        }

        public async Task SeedAsync(this TSoundDbContext dbContext , IServiceProvider serviceProvider)
        {
            this.genreService = new GenreService(this.unitOfWork);
            await this.genreService.LoadGenresInDbAsync();
        }
    }
}
