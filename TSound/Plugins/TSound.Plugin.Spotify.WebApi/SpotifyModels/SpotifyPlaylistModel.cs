﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TSound.Plugin.Spotify.WebApi.SpotifyModels
{
    public class SpotifyPlaylistModel
    {
        /// <summary>Playlist object (full)</summary>
        /// <remarks>
        /// https://developer.spotify.com/documentation/web-api/reference/object-model/#playlist-object-full 
        /// </remarks>
        public partial class SpotifyPlaylist
        {
            /// <summary>
            /// Returns true if context is not search and the owner allows other users to modify the playlist. 
            /// Otherwise returns false.
            /// </summary>
            [JsonProperty("collaborative")]
            public bool Collaborative { get; set; }

            /// <summary>
            /// The playlist description. Only returned for modified, verified playlists, otherwise null.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// Known external URLs for this playlist.
            /// </summary>
            [JsonProperty("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            /// <summary>
            /// Information about the followers of the playlist.
            /// </summary>
            [JsonProperty("followers")]
            public Followers Followers { get; set; }

            /// <summary>
            /// A link to the Web API endpoint providing full details of the playlist.
            /// </summary>
            [JsonProperty("href")]
            public string Href { get; set; }

            /// <summary>
            /// The Spotify ID for the playlist.
            /// </summary>
            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// Images for the playlist. The array may be empty or contain up to three images. The images 
            /// are returned by size in descending order. See Working with Playlists.Note: If returned, 
            /// the source URL for the image ( url ) is temporary and will expire in less than a day.
            /// </summary>
            [JsonProperty("images")]
            public Image[] Images { get; set; }

            /// <summary>
            /// The name of the playlist.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// The user who owns the playlist
            /// </summary>
            [JsonProperty("owner")]
            public SpotifyUser Owner { get; set; }

            [JsonProperty("primary_color")]
            public object PrimaryColor { get; set; }

            /// <summary>
            /// The playlist’s public/private status: true the playlist is public, false the playlist is 
            /// private, null the playlist status is not relevant. 
            /// </summary>
            [JsonProperty("public")]
            public bool? Public { get; set; }

            /// <summary>
            /// The version identifier for the current playlist. 
            /// </summary>
            [JsonProperty("snapshot_id")]
            public string SnapshotId { get; set; }

            /// <summary>
            /// Information about the tracks of the playlist.
            /// </summary>
            [JsonProperty("tracks")]
            public Paged<SpotifyPlaylistTrack> Tracks { get; set; }

            /// <summary>
            /// The object type: “playlist”
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// The Spotify URI for the playlist.
            /// </summary>
            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        /// <summary>Playlist object (simplified)</summary>
        /// <remarks>
        /// https://developer.spotify.com/documentation/web-api/reference/object-model/#playlist-object-simplified 
        /// </remarks>
        public partial class PlaylistSimplified
        {
            /// <summary>
            /// Returns true if context is not search and the owner allows other users to modify the playlist. 
            /// Otherwise returns false.
            /// </summary>
            [JsonProperty("collaborative")]
            public bool Collaborative { get; set; }

            /// <summary>
            /// The playlist description. Only returned for modified, verified playlists, otherwise null.
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            /// <summary>
            /// Known external URLs for this playlist.
            /// </summary>
            [JsonProperty("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            /// <summary>
            /// A link to the Web API endpoint providing full details of the playlist.
            /// </summary>
            [JsonProperty("href")]
            public string Href { get; set; }

            /// <summary>
            /// The Spotify ID for the playlist.
            /// </summary>
            [JsonProperty("id")]
            public string Id { get; set; }

            /// <summary>
            /// Images for the playlist. The array may be empty or contain up to three images. The images 
            /// are returned by size in descending order. See Working with Playlists. Note: If returned, 
            /// the source URL for the image(url ) is temporary and will expire in less than a day.
            /// </summary>
            [JsonProperty("images")]
            public Image[] Images { get; set; }

            /// <summary>
            /// The name of the playlist.
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// The user who owns the playlist
            /// </summary>
            [JsonProperty("owner")]
            public SpotifyUser Owner { get; set; }

            [JsonProperty("primary_color")]
            public object PrimaryColor { get; set; }

            /// <summary>
            /// The playlist’s public/private status: true the playlist is public, false the playlist is 
            /// private, null the playlist status is not relevant. 
            /// </summary>
            [JsonProperty("public")]
            public bool? Public { get; set; }

            /// <summary>
            /// The version identifier for the current playlist.
            /// </summary>
            [JsonProperty("snapshot_id")]
            public string SnapshotId { get; set; }

            /// <summary>
            /// A collection containing a link ( href ) to the Web API endpoint where full details of the 
            /// playlist’s tracks can be retrieved, along with the total number of tracks in the playlist.
            /// </summary>
            [JsonProperty("tracks")]
            public PlaylistTracks Tracks { get; set; }

            /// <summary>
            /// The object type: “playlist”
            /// </summary>
            [JsonProperty("type")]
            public string Type { get; set; }

            /// <summary>
            /// The Spotify URI for the playlist.
            /// </summary>
            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        /// <summary>
        /// A collection containing a link ( href ) to the Web API endpoint where full details of the playlist’s 
        /// tracks can be retrieved, along with the total number of tracks in the playlist.
        /// </summary>
        /// <remarks>
        /// https://developer.spotify.com/documentation/web-api/reference/object-model/#playlist-object-simplified
        /// </remarks>
        public partial class PlaylistTracks
        {
            [JsonProperty("href")]
            public string Href { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }
        }

        /// <summary>
        /// Playlist track object
        /// </summary>
        /// <remarks>
        /// https://developer.spotify.com/documentation/web-api/reference/object-model/#playlist-track-object
        /// </remarks>
        public partial class SpotifyPlaylistTrack
        {
            /// <summary>
            /// The date and time the track was added. Note that some very old playlists may return null 
            /// in this field.
            /// </summary>
            [JsonProperty("added_at")]
            public DateTimeOffset? AddedAt { get; set; }

            /// <summary>
            /// The Spotify user who added the track. Note that some very old playlists may return null 
            /// in this field.
            /// </summary>
            [JsonProperty("added_by")]
            public SpotifyUser AddedBy { get; set; }

            /// <summary>
            /// Whether this track is a local file or not.
            /// </summary>
            [JsonProperty("is_local")]
            public bool IsLocal { get; set; }

            [JsonProperty("primary_color")]
            public string PrimaryColor { get; set; }

            /// <summary>
            /// Information about the track.
            /// </summary>
            [JsonProperty("track")]
            public SpotifyTrack Track { get; set; }

            [JsonProperty("video_thumbnail")]
            public VideoThumbnail VideoThumbnail { get; set; }
        }

        public partial class PlaylistPaged
        {
            [JsonProperty("href")]
            public string Href { get; set; }

            [JsonProperty("items")]
            public SpotifyPlaylistTrack[] Items { get; set; }

            [JsonProperty("limit")]
            public int Limit { get; set; }

            [JsonProperty("next")]
            public string Next { get; set; }

            [JsonProperty("offset")]
            public int Offset { get; set; }

            [JsonProperty("previous")]
            public string Previous { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }
        }

        /// <summary>
        /// Upon adding items to a playlist a snapshot_id is returned.
        /// </summary>
        public partial class PlaylistSnapshotID
        {
            /// <summary>
            /// A snapshot_id which identifies a version of the playlist for future requests.
            /// </summary>
            [JsonProperty("snapshot_id")]
            public string SnapshotID { get; set; }
        }
    }
}
