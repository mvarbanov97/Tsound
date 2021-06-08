using AutoMapper;
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
        private UnitOfWork.UnitOfWork unitOfWork;
        private IMapper mapper;

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
            this.songsService = new SongService(unitOfWork, mapper);
            await this.songsService.LoadSongsInDbAsync();
        }
    }
}
