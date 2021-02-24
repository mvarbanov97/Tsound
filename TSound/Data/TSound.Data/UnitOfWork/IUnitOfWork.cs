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

        Task<int> CompleteAsync();
    }
}
