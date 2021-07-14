using Microsoft.AspNetCore.Identity;
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

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<AlbumArtist> AlbumArtists { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Track> Tracks { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<PlaylistCategory> PlaylistsCategories { get; set; }

        public DbSet<PlaylistTrack> PlaylistTracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            modelBuilder.Entity<User>().ToTable("Users", "dbo");
        }
    }
}
