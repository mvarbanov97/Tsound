using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.Repositories.Contracts;

namespace TSound.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Genre> Genres { get; }

        IRepository<Song> Songs { get; }

        IRepository<Album> Albums { get; }

        IRepository<Artist> Artists { get; }

        IRepository<User> Users { get; }
        IRepository<IdentityUserLogin<Guid>> UserLogins { get; }
        IRepository<IdentityUserToken<Guid>> UserTokens { get; }

        IRepository<Playlist> Playlists { get; }

        IRepository<PlaylistGenre> PlaylistsGenres { get; }

        IRepository<PlaylistSong> PlaylistsSongs { get; }

        Task<int> CompleteAsync();
    }
}
