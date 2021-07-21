using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TSound.Data;
using TSound.Data.UnitOfWork;
using TSound.Plugin.Spotify.WebApi;
using TSound.Plugin.Spotify.WebApi.Authorization;
using TSound.Plugin.Spotify.WebApi.Contracts;
using TSound.Services.Contracts;
using TSound.Services.Providers;

namespace TSound.Services.Tests.Infrastructure
{
    public abstract class BaseTest<T>
        where T : class, IService, new()
    {
        protected readonly TSoundDbContext dbContext;
        protected readonly IMapper mapper;
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IDateTimeProvider dateTimeProvider;
        protected readonly HttpClient httpClient;
        protected readonly IBrowseApi browseApi;
        protected readonly IPlaylistApi playlistApi;
        protected readonly Mock<T> sut;
        protected readonly IConfiguration configuration;
        protected readonly IAccountsService accountsService;

        public string AccessToken { get; set; }

        public BaseTest()
        {
            this.dbContext = TSoundDbContextFactory.Create();
            this.mapper = AutoMapperFactory.Create();
            this.unitOfWork = new UnitOfWork(this.dbContext);
            this.dateTimeProvider = new DateTimeProvider();
            this.httpClient = new HttpClient();
            this.configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json").Build();
            this.accountsService = new AccountsService(this.httpClient, this.configuration);

            this.GetAccessTokenInit();

            this.browseApi = new BrowseApi(this.httpClient, this.AccessToken);
            this.playlistApi = new PlaylistApi(this.httpClient, this.AccessToken);

            if (typeof(ICategoryService).IsAssignableFrom(typeof(T)))
            {
                sut = new Mock<T>(this.unitOfWork, this.mapper, this.browseApi);
            }
            else if (typeof(IPlaylistService).IsAssignableFrom(typeof(T)))
            {
                sut = new Mock<T>(this.unitOfWork, this.dateTimeProvider, this.mapper, this.playlistApi, this.browseApi);
            }
        }

        private void GetAccessTokenInit()
        {
            this.AccessToken = this.accountsService.GetAccessToken().Result;
        }
    }
}
