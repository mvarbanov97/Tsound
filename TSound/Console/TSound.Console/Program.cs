namespace TSound.Console
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using TSound.Data;
    using TSound.Data.UnitOfWork;
    using TSound.Services;
    using TSound.Web.MappingConfiguration;

    public class Program
    {
        static async Task Main()
        {
            using var db = new TSoundDbContext(new DbContextOptionsBuilder<TSoundDbContext>()
            .UseSqlServer("Server=DESKTOP-8E5EDGB\\SQLEXPRESS;Database=TSoundDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options);

            var unitOfWork = new UnitOfWork(db);
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = new Mapper(configuration);


            var genreService = new GenreService(unitOfWork, mapper);
            await genreService.LoadGenresInDbAsync();

            var songService = new SongService(unitOfWork, mapper);
            await songService.LoadSongsInDbAsync(); // This takes quite a while, because we must not exceed request quota for Deezer
        }
    }
}

