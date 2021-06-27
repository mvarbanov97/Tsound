using Microsoft.AspNetCore.Identity;
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
            this.Categories = new Repository<Category>(context);
            this.Tracks = new Repository<Track>(context);
            this.Albums = new Repository<Album>(context);
            this.Artists = new Repository<Artist>(context);
            this.Users = new Repository<User>(context);
            this.UserLogins = new Repository<IdentityUserLogin<Guid>>(context);
            this.UserTokens = new Repository<IdentityUserToken<Guid>>(context);
            this.Playlists = new Repository<Playlist>(context);
            this.PlaylistCategories = new Repository<PlaylistCategory>(context);
            this.PlaylistTracks = new Repository<PlaylistTrack>(context);
        }

        public IRepository<Category> Categories { get; private set; }

        public IRepository<Track> Tracks { get; private set; }

        public IRepository<Album> Albums { get; private set; }

        public IRepository<Artist> Artists { get; private set; }

        public IRepository<User> Users { get; private set; }
        
        public IRepository<IdentityUserLogin<Guid>> UserLogins { get; private set; }

        public IRepository<IdentityUserToken<Guid>> UserTokens { get; private set; }

        public IRepository<Playlist> Playlists { get; private set; }

        public IRepository<PlaylistCategory> PlaylistCategories { get; private set; }

        public IRepository<PlaylistTrack> PlaylistTracks { get; private set; }

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
