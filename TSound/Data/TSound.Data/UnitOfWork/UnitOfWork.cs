using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Repositories;
using TSound.Data.Repositories.Contracts;

namespace TSound.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TSoundDbContext Context;

        public UnitOfWork(TSoundDbContext context)
        {
            this.Context = context;
            this.Genres = new Repository<Genre>(context);
            this.Songs = new Repository<Song>(context);
            this.Albums = new Repository<Album>(context);
            this.Artists = new Repository<Artist>(context);
            this.Users = new Repository<User>(context);
            this.Playlists = new Repository<Playlist>(context);
            this.PlaylistsGenres = new Repository<PlaylistGenre>(context);
            this.PlaylistsSongs = new Repository<PlaylistSong>(context);
        }

        public IRepository<Genre> Genres { get; private set; }

        public IRepository<Song> Songs { get; private set; }

        public IRepository<Album> Albums { get; private set; }

        public IRepository<Artist> Artists { get; private set; }

        public IRepository<User> Users { get; private set; }

        public IRepository<Playlist> Playlists { get; private set; }

        public IRepository<PlaylistGenre> PlaylistsGenres { get; private set; }

        public IRepository<PlaylistSong> PlaylistsSongs { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await this.Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }
    }
}
