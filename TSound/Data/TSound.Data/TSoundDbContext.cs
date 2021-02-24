using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TSound.Data.Models;

namespace TSound.Data
{
    public class TSoundDbContext : IdentityDbContext<User, Role, Guid>
    {
        public TSoundDbContext(DbContextOptions<TSoundDbContext> options)
            : base(options)
        {

        }

        public DbSet<Genre> Genres { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            modelBuilder.Entity<User>().ToTable("Users", "dbo");
            
        }
    }
}
