using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services;

namespace TSound.Data.Seeder.Seeding
{
    public class SongsSeeder : ISeeder
    {
        private SongService songsService;
        private readonly TSoundDbContext dbContext;
        private UnitOfWork.UnitOfWork unitOfWork;

        public SongsSeeder(TSoundDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.unitOfWork = new UnitOfWork.UnitOfWork(this.dbContext);
        }

        public Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
