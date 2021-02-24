using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Repositories.Contracts;

namespace TSound.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected TSoundDbContext Context { get; set; }

        protected readonly DbSet<T> DbSet;

        public Repository(TSoundDbContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await this.DbSet.AddAsync(entity).AsTask();
        }

        public void Delete(T entity)
        {
            this.DbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await this.DbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await this.DbSet.FindAsync(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await this.DbSet.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.Context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }
    }
}
