using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;

namespace TSound.Plugin.Spotify.WebApi.Helpers
{
    public static class SpotifyUriHelper
    {
        public static readonly Regex SpotifyUriRegEx = SpotifyUri.SpotifyIdRegEx;
        public static readonly Regex SpotifyUserPlaylistUriRegEx = SpotifyUri.SpotifyUserPlaylistUriRegEx;
        public static readonly Regex SpotifyIdRegEx = SpotifyUri.SpotifyIdRegEx;

        /// <summary>
        /// Converts a Spotify Track Id or URI into a Spotify URI
        /// </summary>
        public static string TrackUri(string trackId) => ToUri("track", trackId);

        /// <summary>
        /// Converts a Spotify Album Id or URI into a Spotify URI
        /// </summary>
        public static string AlbumUri(string albumId) => ToUri("album", albumId);

        /// <summary>
        /// Converts a Spotify Artist Id or URI into a Spotify URI
        /// </summary>
        public static string ArtistUri(string artistId) => ToUri("artist", artistId);

        /// <summary>
        /// Converts a Spotify Playlist Id or URI into a Spotify URI
        /// </summary>
        public static string PlaylistUri(string playlistId) => ToUri("playlist", playlistId);

        /// <summary>
        /// Converts a Spotify Track Id or URI into a Spotify Id
        /// </summary>
        public static string TrackId(string trackId) => ToId("track", trackId);

        /// <summary>
        /// Converts a Spotify Album Id or URI into a Spotify Id
        /// </summary>
        public static string AlbumId(string albumId) => ToId("album", albumId);

        /// <summary>
        /// Converts a Spotify Artist Id or URI into a Spotify Id
        /// </summary>
        public static string ArtistId(string artistId) => ToId("artist", artistId);

        /// <summary>
        /// Converts a Spotify Playlist Id or URI into a Spotify Id
        /// </summary>
        public static string PlaylistId(string playlistId) => ToId("playlist", playlistId);

        private static string ToUri(string type, string id)
        {
            var uri = new SpotifyUri(id, type);
            if (!uri.IsValid) throw new ArgumentException($"\"{id}\" is not a valid Spotify identifier.");
            return uri.FullUri;
        }

        /// <summary>
        /// Converts any valid spotify URI to the standard Spotify URI format, i.e. spotify:(type):(id)
        /// </summary>
        /// <param name="uri">Any Spotify URI</param>
        /// <returns>The normalized Spotify URI</returns>
        public static string NormalizeUri(string uri)
        {
            var spotifyUri = new SpotifyUri(uri);
            if (!spotifyUri.IsValid || !spotifyUri.IsSpotifyUri)
                throw new ArgumentException($"\"{uri}\" is not a valid Spotify URI.");
            return spotifyUri.Uri;
        }

        internal static string ToId(string type, string idOrUri, bool throwIfNotValid = true)
        {
            var uri = new SpotifyUri(idOrUri, type);
            if (throwIfNotValid && !uri.IsValid)
                throw new ArgumentException($"\"{idOrUri}\" is not a valid Spotify {type} identifier");

            return uri.Id;
        }
    }
}
