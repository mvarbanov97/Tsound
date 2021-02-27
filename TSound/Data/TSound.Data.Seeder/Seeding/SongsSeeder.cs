using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services;
using TSound.Web.MappingConfiguration;

namespace TSound.Data.Seeder.Seeding
{
    public class SongsSeeder : ISeeder
    {
        private SongService songsService;
        private UnitOfWork.UnitOfWork unitOfWork;

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = new Mapper(configuration);

            this.unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
            this.songsService = new SongService(unitOfWork, mapper);
            await this.songsService.LoadSongsInDbAsync();
        }
    }
}
