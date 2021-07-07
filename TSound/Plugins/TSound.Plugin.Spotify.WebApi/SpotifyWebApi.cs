using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using TSound.Data.Models;
using TSound.Plugin.Spotify.WebApi.Authorization;
using TSound.Plugin.Spotify.WebApi.Http;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;

namespace TSound.Plugin.Spotify.WebApi
{
    public abstract class SpotifyWebApi
    {
        protected internal const string BaseUrl = "https://api.spotify.com/v1";
        protected internal readonly HttpClient _http;
        protected readonly string _accessToken;

        #region Constructors
        /// <summary>
        /// This constructor accepts a Spotify access token that will be used for all calls to the API 
        /// (except when an accessToken is provided using the optional `accessToken` parameter on each method).
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/></param>
        /// <param name="accessToken">A valid access token from the Spotify Accounts service</param>
        public SpotifyWebApi(HttpClient httpClient, string accessToken) : this(httpClient)
        {
            if (string.IsNullOrEmpty(accessToken)) throw new ArgumentNullException(nameof(accessToken));
            _accessToken = accessToken;
        }

        /// <summary>
        /// Use this constructor when an accessToken will be provided using the `accessToken` parameter 
        /// on each method
        /// </summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/></param>
        public SpotifyWebApi(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #endregion

        protected internal Task<string> GetAccessToken(string accessToken = null)
        {
            // accessToken or _accessToken or from _tokenProvider or from _accounts
            string token = accessToken;

            if (string.IsNullOrEmpty(token)) throw new InvalidOperationException("Access Token is null. Could not get Access Token.");
            return Task.FromResult(token);
        }

        /// <summary>
        /// Invoke a GET request and deserialise the JSON to a model of T
        /// </summary>
        /// <param name="url">The URL to GET</param>
        /// <typeparam name="T">The type to deserialise to</typeparam>
        protected internal virtual Task<T> GetModel<T>(string url, string accessToken = null)
            => GetModel<T>(new Uri(url), accessToken: accessToken);

        /// <summary>
        /// Invoke a GET request and deserialise the JSON to a model of T
        /// </summary>
        /// <param name="url">The URL to GET</param>
        /// <typeparam name="T">The type to deserialise to</typeparam>
        protected internal virtual async Task<T> GetModel<T>(Uri uri, string accessToken = null)
        {
            return JsonConvert.DeserializeObject<T>
            (
                await _http.Get
                (
                    uri,
                    new AuthenticationHeaderValue("Bearer", accessToken ?? await GetAccessToken())
                )
            );
        }

        /// <summary>
        /// Invoke a GET request and deserialise the result to JSON from a root property of the Spotify Response
        /// </summary>
        /// <param name="url">The URL to GET</param>
        /// <param name="rootPropertyName">The name of the root property of the JSON response to deserialise, e.g. "artists"</param>
        /// <typeparam name="T">The type to deserialise to</typeparam>
        /// <returns></returns>
        protected internal virtual Task<T> GetModelFromProperty<T>(
            string url,
            string rootPropertyName,
            string accessToken = null)
                => GetModelFromProperty<T>(new Uri(url), rootPropertyName, accessToken: accessToken);

        /// <summary>
        /// Invoke a GET request and deserialise the result to JSON from a root property of the Spotify Response
        /// </summary>
        /// <param name="url">The URL to GET</param>
        /// <param name="rootPropertyName">The name of the root property of the JSON response to deserialise, e.g. "artists"</param>
        /// <typeparam name="T">The type to deserialise to</typeparam>
        /// <returns></returns>
        protected internal virtual async Task<T> GetModelFromProperty<T>(
            Uri uri,
            string rootPropertyName,
            string accessToken = null)
        {
            var jObject = await GetJObject(uri, accessToken: accessToken);
            if (jObject == null) return default;
            return jObject[rootPropertyName].ToObject<T>();
        }

        /// <summary>
        /// GET uri and deserialize as <see cref="JObject"/>
        /// </summary>
        protected internal virtual async Task<JObject> GetJObject(Uri uri, string accessToken = null)
        {
            string json = await _http.Get
            (
                uri,
                new AuthenticationHeaderValue("Bearer", accessToken ?? await GetAccessToken())
            );

            // Todo #25 return 204 no content result 
            if (string.IsNullOrEmpty(json)) return null;

            JObject deserialized = JsonConvert.DeserializeObject(json) as JObject;
            if (deserialized == null)
                throw new InvalidOperationException($"Failed to deserialize response as JSON. Response = {json.Substring(0, Math.Min(json.Length, 256))}");

            return deserialized;
        }

        /// <summary>
        /// Helper to PUT an object as JSON body
        /// </summary>
        [Obsolete("Use UriBuilder to construct Uri using AppendToQuery extensions")]
        protected internal virtual async Task<SpotifyResponse> Put(string url, object data, string accessToken = null)
            => await PostOrPut<dynamic>("PUT", new Uri(url), data, accessToken);

        /// <summary>
        /// Helper to POST an object as JSON body
        /// </summary>
        [Obsolete("Use UriBuilder to construct Uri using AppendToQuery extensions")]
        protected internal virtual async Task<SpotifyResponse> Post(string url, object data, string accessToken = null)
            => await PostOrPut<dynamic>("POST", new Uri(url), data, accessToken);

        /// <summary>
        /// Helper to PUT an object as JSON body
        /// </summary>
        protected internal virtual async Task<SpotifyResponse> Put(Uri uri, object data, string accessToken = null)
            => await PostOrPut<dynamic>("PUT", uri, data, accessToken);

        /// <summary>
        /// Helper to POST an object as JSON body
        /// </summary>
        protected internal virtual async Task<SpotifyResponse> Post(Uri uri, object data, string accessToken = null)
            => await PostOrPut<dynamic>("POST", uri, data, accessToken);

        /// <summary>
        /// Helper to POST an object as JSON body and deserialize response as T
        /// </summary>
        protected internal virtual async Task<SpotifyResponse<T>> Post<T>(Uri uri, object data, string accessToken = null)
            => await PostOrPut<T>("POST", uri, data, accessToken);

        /// <summary>
        /// Helper to DELETE an object
        /// </summary>
        protected internal virtual async Task<SpotifyResponse> Delete(Uri uri, string accessToken = null)
        {

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                accessToken ?? await GetAccessToken());

            var response = await _http.DeleteAsync(uri);

            await RestHttpClient.CheckForErrors(response);

            return new SpotifyResponse
            {
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase
            };
        }

        private async Task<SpotifyResponse<T>> PostOrPut<T>(string verb, Uri uri, object data, string accessToken = null)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken ?? await GetAccessToken());

            _http.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            StringContent content = null;

            if (data == null)
            {
                content = null;
            }
            else
            {
                content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            HttpResponseMessage response = null;

            switch (verb)
            {
                case "PUT":
                    response = await _http.PutAsync(uri, content);
                    break;
                case "POST":
                    response = await _http.PostAsync(uri, content);
                    break;
                default:
                    throw new NotSupportedException($"{verb} is not a supported verb");
            }

            await RestHttpClient.CheckForErrors(response);

            var spotifyResponse = new SpotifyResponse<T>
            {
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase
            };

            if (response.Content != null)
            {
                string json = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json)) spotifyResponse.Data = JsonConvert.DeserializeObject<T>(json);
            }

            return spotifyResponse;
        }
    }
}
