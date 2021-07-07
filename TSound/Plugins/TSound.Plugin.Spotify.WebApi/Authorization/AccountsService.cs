using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.UnitOfWork;
using TSound.Plugin.Spotify.WebApi.Http;

namespace TSound.Plugin.Spotify.WebApi.Authorization
{
    public class AccountsService : IAccountsService
    {
        protected const string TokenUrl = "https://accounts.spotify.com/api/token";

        protected readonly HttpClient _http;
        protected readonly IConfiguration _config;
        protected readonly IUnitOfWork _unitOfWork;

        #region constructors

        /// <summary>
        /// Instantiates an AccountsService class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> for making HTTP calls to the Spotify Accounts Service.</param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/> fsor providing Configuration.</param>
        /// <param name="bearerTokenStore">An instance of <see cref="IBearerTokenStore"/> for storing cached Access (Bearer) tokens.</param>
        public AccountsService(HttpClient httpClient, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            if (httpClient == null) throw new ArgumentNullException("httpClient");
            _http = httpClient;

            // if configuration is not provided, read from environment variables

            _config = configuration ?? new ConfigurationBuilder()
                .Build();

            ValidateConfig();

            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Instantiates an AccountsService class.
        /// </summary>
        public AccountsService() : this(new HttpClient(), null, null) { }

        /// <summary>
        /// Instantiates an AccountsService class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> for making HTTP calls to the Spotify Accounts Service.</param>
        public AccountsService(HttpClient httpClient) : this(httpClient, null, null) { }

        /// <summary>
        /// Instantiates an AccountsService class.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> for making HTTP calls to the Spotify Accounts Service.</param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/> for providing Configuration.</param>
        public AccountsService(HttpClient httpClient, IConfiguration configuration) : this(httpClient, configuration, null) { }

        #endregion

        /// <summary>
        /// Returns a valid access token for the Spotify Service.
        /// </summary>
        /// <returns>The token as string.</returns>
        public async Task<string> GetAccessToken()
        {
            var token = await GetAccessToken(_config["Spotify:ClientId"], "grant_type=client_credentials");
            return token.AccessToken;
        }

        protected async Task<BearerAccessToken> GetAccessToken(string tokenKey, string body)
        {
            var now = DateTime.UtcNow;

            string json = await _http.Post(new Uri(TokenUrl), body, GetHeader(_config));

            // deserialise the token
            var newToken = JsonConvert.DeserializeObject<BearerAccessToken>(json);
            // set absolute expiry
            newToken.SetExpires(now);

            // add to store
            newToken.EnforceInvariants();
            return newToken;
        }

        protected async Task<BearerAccessToken> RefreshAccessToken(string body)
        {
            var now = DateTime.UtcNow;
            string json = await _http.Post(new Uri(TokenUrl), body, GetHeader(_config));
            // deserialise the token
            var newToken = JsonConvert.DeserializeObject<BearerAccessToken>(json);
            // set absolute expiry
            newToken.SetExpires(now);
            return newToken;
        }

        protected static AuthenticationHeaderValue GetHeader(IConfiguration configuration)
        {
            return new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}",
                    configuration["Spotify:ClientId"], configuration["Spotify:ClientSecret"])))
            );
        }

        private void ValidateConfig()
        {
            if (string.IsNullOrEmpty(_config["Spotify:ClientId"]))
                throw new ArgumentNullException("Spotify:ClientId", "Expecting configuration value for `Spotify:ClientId`");
            if (string.IsNullOrEmpty(_config["Spotify:ClientSecret"]))
                throw new ArgumentNullException("Spotify:ClientSecret", "Expecting configuration value for `Spotify:ClientSecret`");
        }
    }
}
