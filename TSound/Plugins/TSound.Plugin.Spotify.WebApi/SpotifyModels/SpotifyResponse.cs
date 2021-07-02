using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace TSound.Plugin.Spotify.WebApi.SpotifyModels
{
    public class SpotifyResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ReasonPhrase { get; set; }
    }

    /// <summary>
    /// A generic response from the Spotify API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpotifyResponse<T> : SpotifyResponse
    {
        public T Data { get; set; }
    }
}
