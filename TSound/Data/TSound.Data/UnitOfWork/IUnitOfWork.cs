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

        Task<int> CompleteAsync();
    }
}
