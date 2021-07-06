﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.UnitOfWork;
using TSound.Plugin.Spotify.WebApi.Http;

namespace TSound.Plugin.Spotify.WebApi.Authorization
{
    /// <summary>
    /// Spotify Accounts Service for the User (Authorization Code) Flow.
    /// </summary>
    /// <remarks>https://developer.spotify.com/documentation/general/guides/authorization-guide/#authorization-code-flow</remarks>
    public class UserAccountsService : AccountsService, IUserAccountsService
    {
        private const string AccountsAuthorizeUrl = "https://accounts.spotify.com/authorize";

        private readonly IUnitOfWork _unitOfWork;

        #region constructors

        /// <summary>
        /// Instantiate a new <see cref="UserAccountsService"/>.
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/>.</param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/>.</param>
        public UserAccountsService(HttpClient httpClient, IConfiguration configuration, IUnitOfWork unitOfWork)
            : base(httpClient, configuration)
        {
            ValidateConfig();
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Instantiate a new <see cref="UserAccountsService"/>.
        /// </summary>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/>.</param>
        public UserAccountsService(IConfiguration configuration)
            : base(new HttpClient(), configuration)
        {
            ValidateConfig();
        }

        #endregion

        public async Task<string> GetRefreshTokenFromDb(Guid userId)
        {
            var refreshToken = this._unitOfWork.UserTokens.All().Where(x => x.UserId == userId && x.Name == "refresh_token").Select(x => x.Value).FirstOrDefault();

            return await Task.FromResult(refreshToken);
        }

        /// <summary>
        /// Refresh a Bearer (Access) token when it has expired / is about to expire.
        /// </summary>
        /// <param name="refreshToken">The refresh token returned from the authorization code exchange.</param>
        /// <returns>An instance of <see cref="BearerAccessToken"/>.</returns>
        public async Task<BearerAccessToken> RefreshUserAccessToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException(nameof(refreshToken));

            return await RefreshAccessToken(
                $"grant_type=refresh_token&refresh_token={refreshToken}&redirect_uri={_config["Spotify:AuthRedirectUri"]}");
        }

        /// <summary>
        /// Derives and returns a URL for a webpage where a user can choose to grant your app access to their data.
        /// </summary>
        /// <param name="state">Optional, but strongly recommended. Random state value to provides protection against 
        /// attacks such as cross-site request forgery. See important notes in <see cref="https://developer.spotify.com/documentation/general/guides/authorization-guide/#authorization-code-flow"/></param>
        /// <param name="scopes">Optional. A space-separated list of scopes.</param>
        /// <returns>A fully qualified URL.</returns>
        public string AuthorizeUrl(string state, string[] scopes)
        {
            return AuthorizeUrl(state, scopes, _config["Spotify:ClientId"], _config["Spotify:AuthRedirectUri"]);
        }

        /// <summary>
        /// Derives and returns a URL for a webpage where a user can choose to grant your app access to their data.
        /// </summary>
        /// <param name="state">Optional, but strongly recommended. Random state value to provides protection against 
        /// attacks such as cross-site request forgery. See important notes in <see cref="https://developer.spotify.com/documentation/general/guides/authorization-guide/#authorization-code-flow"/></param>
        /// <param name="scopes">Optional. A space-separated list of scopes.</param>
        /// <param name="spotifyApiClientId">A valid Spotify API Client Id.</param>
        /// <param name="spotifyAuthRedirectUri">A valid Spotify Auth Redirect URI.</param>
        /// <returns>A fully qualified URL.</returns>
        public static string AuthorizeUrl(string state, string[] scopes, string spotifyApiClientId, string spotifyAuthRedirectUri)
        {
            if (string.IsNullOrEmpty(spotifyApiClientId)) throw new ArgumentNullException(nameof(spotifyApiClientId));
            if (string.IsNullOrEmpty(spotifyAuthRedirectUri)) throw new ArgumentNullException(nameof(spotifyAuthRedirectUri));

            string scope = scopes == null || scopes.Length == 0 ? "" : string.Join("%20", scopes);
            return $"{AccountsAuthorizeUrl}/?client_id={spotifyApiClientId}&response_type=code&redirect_uri={spotifyAuthRedirectUri}&scope={scope}&state={state}";
        }

        /// <summary>
        /// Exchange the authorization code returned by the `/authorize` endpoint for a <see cref="BearerAccessRefreshToken"/>.
        /// </summary>
        /// <param name="code">The authorization code returned from the initial request to the Account /authorize endpoint.</param>
        /// <returns>An instance of <see cref="BearerAccessRefreshToken"/></returns>
        public async Task<BearerAccessRefreshToken> RequestAccessRefreshToken(string code)
        {
            var now = DateTime.UtcNow;
            // POST the code to get the tokens
            var token = await GetAuthorizationTokens(code);
            // set absolute expiry
            token.SetExpires(now);
            token.EnforceInvariants();
            return token;
        }

        protected internal virtual async Task<BearerAccessRefreshToken> GetAuthorizationTokens(string code)
        {
            var result = await _http.Post(new Uri(TokenUrl),
                $"grant_type=authorization_code&code={code}&redirect_uri={_config["Spotify:AuthRedirectUri"]}",
                GetHeader(_config));
            return JsonConvert.DeserializeObject<BearerAccessRefreshToken>(result);
        }

        private void ValidateConfig()
        {
            if (string.IsNullOrEmpty(_config["Spotify:AuthRedirectUri"]))
                throw new ArgumentNullException("Spotify:AuthRedirectUri", "Expecting configuration value for `Spotify:AuthRedirectUri`");
        }
    }
}