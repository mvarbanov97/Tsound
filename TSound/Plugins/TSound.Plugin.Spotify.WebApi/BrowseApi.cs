using System;
using System.Net.Http;
using System.Threading.Tasks;
using TSound.Plugin.Spotify.WebApi.Authorization;
using TSound.Plugin.Spotify.WebApi.Contracts;
using TSound.Plugin.Spotify.WebApi.Extensions;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;

namespace TSound.Plugin.Spotify.WebApi
{
    public class BrowseApi : SpotifyWebApi, IBrowseApi
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new <see cref="BrowseApi"/>.
        /// </summary>
        /// <remarks>
        /// Use this constructor when an accessToken will be provided using the `accessToken` parameter 
        /// on each method
        /// </remarks>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/></param>
        public BrowseApi(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="BrowseApi"/>.
        /// </summary>
        /// <remarks>
        /// This constructor accepts a Spotify access token that will be used for all calls to the API 
        /// (except when an accessToken is provided using the optional `accessToken` parameter on each method).
        /// </remarks>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/></param>
        /// <param name="accessToken">A valid access token from the Spotify Accounts service</param>
        public BrowseApi(HttpClient httpClient, string accessToken) : base(httpClient, accessToken)
        {
        }

        #endregion

        /// <summary>
        /// Get a list of categories used to tag items in Spotify (on, for example, the Spotify player's "Browse" tab).
        /// </summary>
        /// <param name="country">Optional. A country: a <see cref="SpotifyCountryCodes"/>. Provide 
        /// this parameter to ensure that the category exists for a particular country.</param>
        /// <param name="locale">Optional. The desired language, consisting of an ISO 639-1 language 
        /// code and an ISO 3166-1 alpha-2 country code, joined by an underscore. For example: es_MX, 
        /// meaning "Spanish (Mexico)". Provide this parameter if you want the category strings returned 
        /// in a particular language. Note that, if locale is not supplied, or if the specified language 
        /// is not available, the category strings returned will be in the Spotify default language 
        /// (American English).</param>
        /// <param name="limit">Optional. Maximum number of results to return. Default: 20, Minimum: 1,
        /// Maximum: 50.</param>
        /// <param name="offset">Optional. The index of the first result to return. Default: 0 (the
        /// first result). Use with limit to get the next page of search results.</param>
        /// <param name="accessToken">Optional. A valid access token from the Spotify Accounts service,
        /// used for this call only. See constructors for other ways to provide an access token.</param>
        /// <returns>A <see cref="PagedCategories"/> object</returns>
        /// <remarks> https://developer.spotify.com/documentation/web-api/reference/browse/get-list-categories/ </remarks>
        public Task<PagedCategories> GetCategories(
            string country = null,
            string locale = null,
            int? limit = null,
            int offset = 0,
            string accessToken = null) => GetCategories<PagedCategories>(
                country: country,
                locale: locale,
                limit: limit,
                offset: offset,
                accessToken: accessToken);

        /// <summary>
        /// Get a list of categories used to tag items in Spotify (on, for example, the Spotify player's "Browse" tab).
        /// </summary>
        /// <typeparam name="T">Type to deserialise result to.</typeparam>
        /// <param name="country">Optional. A country: a <see cref="SpotifyCountryCodes"/>. Provide 
        /// this parameter to ensure that the category exists for a particular country.</param>
        /// <param name="locale">Optional. The desired language, consisting of an ISO 639-1 language 
        /// code and an ISO 3166-1 alpha-2 country code, joined by an underscore. For example: es_MX, 
        /// meaning "Spanish (Mexico)". Provide this parameter if you want the category strings returned 
        /// in a particular language. Note that, if locale is not supplied, or if the specified language 
        /// is not available, the category strings returned will be in the Spotify default language 
        /// (American English).</param>
        /// <param name="limit">Optional. Maximum number of results to return. Default: 20, Minimum: 1,
        /// Maximum: 50.</param>
        /// <param name="offset">Optional. The index of the first result to return. Default: 0 (the
        /// first result). Use with limit to get the next page of search results.</param>
        /// <param name="accessToken">Optional. A valid access token from the Spotify Accounts service,
        /// used for this call only. See constructors for other ways to provide an access token.</param>
        /// <returns>Result deserialised to `T`.</returns>
        /// <remarks> https://developer.spotify.com/documentation/web-api/reference/browse/get-list-categories/ </remarks>
        public async Task<T> GetCategories<T>(string country = null, string locale = null, int? limit = null, int offset = 0, string accessToken = null)
        {
            var builder = new UriBuilder($"{BaseUrl}/browse/categories");
            builder.AppendToQueryIfValueNotNullOrWhiteSpace("country", country);
            builder.AppendToQueryIfValueNotNullOrWhiteSpace("locale", locale);
            builder.AppendToQueryIfValueGreaterThan0("limit", limit);
            builder.AppendToQueryIfValueGreaterThan0("offset", offset);
            return await GetModelFromProperty<T>(builder.Uri, "categories", accessToken: accessToken);
        }

        /// <summary>
        /// Get a list of Spotify playlists tagged with a particular category.
        /// </summary>
        /// <param name="categoryId">The Spotify category ID for the category.</param>
        /// <param name="country">Optional. A country: a <see cref="SpotifyCountryCodes"/>. Provide 
        /// this parameter to ensure that the category exists for a particular country.</param>
        /// <param name="limit">Optional. Maximum number of results to return. Default: 20, Minimum: 1,
        /// Maximum: 50.</param>
        /// <param name="offset">Optional. The index of the first result to return. Default: 0 (the
        /// first result). Use with limit to get the next page of search results.</param>
        /// <param name="accessToken">Optional. A valid access token from the Spotify Accounts service,
        /// used for this call only. See constructors for other ways to provide an access token.</param>
        /// <returns>A <see cref="PagedPlaylists"/> object</returns>
        /// <remarks> https://developer.spotify.com/documentation/web-api/reference/browse/get-categorys-playlists/ </remarks>
        public Task<PagedPlaylists> GetCategoryPlaylists(
            string categoryId,
            string country = null,
            int? limit = null,
            int offset = 0,
            string accessToken = null) => GetCategoryPlaylists<PagedPlaylists>(
                categoryId,
                country: country,
                limit: limit,
                offset: offset,
                accessToken: accessToken);

        /// <summary>
        /// Get a list of Spotify playlists tagged with a particular category.
        /// </summary>
        /// <typeparam name="T">Type to deserialise result to.</typeparam>
        /// <param name="categoryId">The Spotify category ID for the category.</param>
        /// <param name="country">Optional. A country: a <see cref="SpotifyCountryCodes"/>. Provide 
        /// this parameter to ensure that the category exists for a particular country.</param>
        /// <param name="limit">Optional. Maximum number of results to return. Default: 20, Minimum: 1,
        /// Maximum: 50.</param>
        /// <param name="offset">Optional. The index of the first result to return. Default: 0 (the
        /// first result). Use with limit to get the next page of search results.</param>
        /// <param name="accessToken">Optional. A valid access token from the Spotify Accounts service,
        /// used for this call only. See constructors for other ways to provide an access token.</param>
        /// <returns>Result deserialised to `T`.</returns>
        /// <remarks> https://developer.spotify.com/documentation/web-api/reference/browse/get-categorys-playlists/ </remarks>
        public async Task<T> GetCategoryPlaylists<T>(
            string categoryId,
            string country = null,
            int? limit = null,
            int offset = 0,
            string accessToken = null)
        {
            var builder = new UriBuilder($"{BaseUrl}/browse/categories/{categoryId}/playlists");
            builder.AppendToQueryIfValueNotNullOrWhiteSpace("country", country);
            builder.AppendToQueryIfValueGreaterThan0("limit", limit);
            builder.AppendToQueryIfValueGreaterThan0("offset", offset);
            return await GetModelFromProperty<T>(builder.Uri, "playlists", accessToken: accessToken);
        }
    }
}
