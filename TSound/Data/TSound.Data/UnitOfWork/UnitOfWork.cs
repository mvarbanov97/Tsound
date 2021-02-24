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
        }

        public IRepository<Genre> Genres { get; private set; }

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
