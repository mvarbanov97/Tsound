using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Models.DeezerModels;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.Models;

namespace TSound.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SongService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<SongServiceModel> GetSongByIdAsync(Guid songId)
        {
            var song = await this.unitOfWork.Songs.All()
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(s => s.Id == songId);

            if (song == null)
                throw new ArgumentNullException("Song Not Found.");

            var songServiceModel = this.mapper.Map<SongServiceModel>(song);

            return songServiceModel;
        }

        public async Task<IEnumerable<SongServiceModel>> GetAllSongsAsync(Guid songId)
        {
            var songs = this.unitOfWork.Songs.All()
                .Include(s => s.Album)
                .Include(s => s.Artist)
                .Include(s => s.Genre);

            var songServiceModels = this.mapper.Map<IEnumerable<SongServiceModel>>(songs);

            return songServiceModels;
        }

        public async Task LoadSongsInDbAsync()
        {
            List<Song> songs = await this.GetSongsForAllGenres();
            songs = await this.LoadSongArtistsInDbAsync(songs);
            songs = await this.LoadSongAlbumsInDbAsync(songs);

            foreach (var song in songs)
            {
                song.Artist = null;
                song.Album = null;
                await this.unitOfWork.Songs.AddAsync(song);
            }

            await this.unitOfWork.CompleteAsync();
        }

        private async Task<List<Song>> LoadSongAlbumsInDbAsync(List<Song> songs)
        {
            Album songAlbum;

            foreach (var song in songs)
            {
                songAlbum = await this.unitOfWork.Albums.All().FirstOrDefaultAsync(a => a.DeezerId == song.Album.DeezerId);

                if (songAlbum == null)
                {
                    song.Album.ArtistId = song.ArtistId;

                    await this.unitOfWork.Albums.AddAsync(song.Album);
                    await this.unitOfWork.CompleteAsync();
                    songAlbum = await this.unitOfWork.Albums.All().FirstAsync(a => a.DeezerId == song.Album.DeezerId);
                }

                song.AlbumId = songAlbum.Id;
            }

            return songs;
        }

        private async Task<List<Song>> LoadSongArtistsInDbAsync(List<Song> songs)
        {
            Artist songArtist;

            foreach (var song in songs)
            {
                songArtist = await this.unitOfWork.Artists.All().FirstOrDefaultAsync(a => a.DeezerId == song.Artist.DeezerId);

                if(songArtist == null)
                {
                    await this.unitOfWork.Artists.AddAsync(song.Artist);
                    await this.unitOfWork.CompleteAsync();

                    songArtist = await this.unitOfWork.Artists.All().FirstAsync(a => a.DeezerId == song.Artist.DeezerId);
                }

                song.ArtistId = songArtist.Id;
            }

            return songs;
        }

        private async Task<List<Song>> GetSongsForAllGenres()
        {
            List<Song> songs = new List<Song>();
            List<DeezerPlaylist> playlists;
            List<string> trackListsUrls;
            List<Song> songsFromTrackLists;

            foreach (var genre in this.unitOfWork.Genres.All())
            {
                playlists = await this.GetPlaylistsOfGenreFromDeezerAPIAsync(genre.Name);
                trackListsUrls = this.GetTracklistsURLsOfPlaylists(playlists);
                songsFromTrackLists = await this.GetSongsFromTracklistsAsync(trackListsUrls);

                songsFromTrackLists.ForEach(s => s.GenreId = genre.Id); //ToDo : setting songs' genres here, does it work ? 
                //ToDo : We can set the GenreId here, so that later we don't have to send requests to the database to fing each song's genre

                songs.AddRange(songsFromTrackLists);

                Thread.Sleep(5100); // Careful not to exceed request quota, each loop generates 40 requests
            }

            return songs;
        }

        private async Task<List<Song>> GetSongsFromTracklistsAsync(List<string> trackListsUrls)
        {
            List<Song> songs = new List<Song>();
            string jsonTrackList;
            DeezerTrackList trackList;

            foreach (var url in trackListsUrls)
            {
                jsonTrackList = await this.GetJsonStreamFromUrlAsync(url);
                trackList = JsonConvert.DeserializeObject<DeezerTrackList>(jsonTrackList);

                songs.AddRange(trackList.Songs);

                if (songs.Count >= 300) // we don't need more than 300 songs per Genre, would make queries slow, otherwise we would remove this
                    break;
            }

            return songs;
        }

        private List<string> GetTracklistsURLsOfPlaylists(List<DeezerPlaylist> playlists)
        {
            List<string> trackListsURLs = new List<string>();

            foreach (var item in playlists)
            {
                trackListsURLs.Add(item.SongListURL);
                if (trackListsURLs.Count == 40) //reducing risk of exceeding request quota in other methods
                    break;
            }

            return trackListsURLs;
        }

        private async Task<List<DeezerPlaylist>> GetPlaylistsOfGenreFromDeezerAPIAsync(string genre)
        {
            string url = $"https://api.deezer.com/search/playlist?q={genre}";

            string jsonPlaylist = await this.GetJsonStreamFromUrlAsync(url);

            DeezerPlaylistData playlistsData = JsonConvert.DeserializeObject<DeezerPlaylistData>(jsonPlaylist);
            var playlists = playlistsData.Playlists;


            return playlists;
        }

    }
}
