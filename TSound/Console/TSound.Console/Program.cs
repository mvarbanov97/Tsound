namespace TSound.Console
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using TSound.Data;
    using TSound.Data.UnitOfWork;
    using TSound.Services;
    using TSound.Services.External.SpotifyAuthorization;
    using TSound.Web.MappingConfiguration;

    public class Program
    {
        public static async Task Main()
        {
            using var db = new TSoundDbContext(new DbContextOptionsBuilder<TSoundDbContext>()
            .UseSqlServer("Server=DESKTOP-8E5EDGB\\SQLEXPRESS;Database=TSoundDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options);

            var http = new HttpClient();
            var unitOfWork = new UnitOfWork(db);
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutomapperProfile>());
            var mapper = new Mapper(mapperConfig);
            var accountsService = new AccountsService();
            var configuraton = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var genreService = new GenreService(unitOfWork, mapper,http, configuraton, accountsService);

            var songService = new SongService(unitOfWork, mapper);
            await songService.LoadSongsInDbAsync(); // This takes quite a while, because we must not exceed request quota for Deezer
        }
    }
}

