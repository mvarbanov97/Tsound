using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services;

namespace TSound.Data.Seeder.Seeding
{
    public class TracksSeeder : ISeeder
    {
        private TrackService tracksService;
        private UnitOfWork.UnitOfWork unitOfWork;
        private IMapper mapper;

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.unitOfWork = new UnitOfWork.UnitOfWork(dbContext);
            this.tracksService = new TrackService(unitOfWork, mapper);
            //await tracksService.AddTracksToDbAsync();
        }
    }
}
