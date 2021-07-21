using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TSound.Data;
using TSound.Data.Models;

namespace TSound.Services.Tests.Infrastructure
{
    public class TSoundDbContextFactory
    {
        public static TSoundDbContext Create()
        {
            var dbContextOptions = new DbContextOptionsBuilder<TSoundDbContext>()
                .UseInMemoryDatabase(DateTime.Now.ToString() + Guid.NewGuid().ToString())
                .Options;

            var dbContext = new TSoundDbContext(dbContextOptions);

            dbContext.Database.EnsureCreated();

            var adminRole = new Role("Admin");
            dbContext.Roles.Add(adminRole);
            dbContext.SaveChanges();

            // Seeding admin user
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "MomchilVarbanov",
                Email = "momchil.varbanov97@gmail.com",
                DateCreated = new DateTime(2019, 07, 03),
            };

            dbContext.Users.Add(adminUser);

            dbContext.SaveChanges();

            dbContext.UserRoles.Add(new IdentityUserRole<Guid>() { RoleId = adminRole.Id, UserId = adminUser.Id });
            dbContext.SaveChanges();

            // Seeding normal users
            var firstUser = new User { Id = Guid.NewGuid(), UserName = "FooUser1", Email = "Foo1@bg.bg", PasswordHash = "asd" };
            var secondUser = new User { Id = Guid.NewGuid(), UserName = "FooUser2", Email = "Foo2@bg.bg", PasswordHash = "asd" };
            var thirdUser = new User { Id = Guid.NewGuid(), UserName = "FooUser3", Email = "Foo3@bg.bg", PasswordHash = "asd" };

            dbContext.AddRange(firstUser, secondUser, thirdUser);

            // Seeding categories
            var firstCategory = new Category { Id = Guid.NewGuid(), Name = "Hip Hop", SpotifyId = "hiphop" };
            var secondCategory = new Category { Id = Guid.NewGuid(), Name = "Chill", SpotifyId = "chill" };
            var thirdCategory = new Category { Id = Guid.NewGuid(), Name = "Rock", SpotifyId = "rock" };

            dbContext.AddRange(firstCategory, secondCategory, thirdCategory);

            // Seeding Playlist
            var playlistOne = new Playlist { Id = Guid.NewGuid(), Name = "First Playlist", Description = "First Description" };
            var playlistTwo = new Playlist { Id = Guid.NewGuid(), Name = "Second Playlist", Description = "Second Description" };
            var playlistThree = new Playlist { Id = Guid.NewGuid(), Name = "Third Playlist", Description = "Third Description" };

            dbContext.AddRange(playlistOne, playlistTwo, playlistThree);

            // Seeding Tracks
            var trackOne = new Track { Id = Guid.NewGuid(), SpotifyId = "testSpotifyIdOne", Name = "Haide pochvai me" };
            var trackTwo = new Track { Id = Guid.NewGuid(), SpotifyId = "testSpotifyIdTwo", Name = "Sen Trope" };
            var trackThree = new Track { Id = Guid.NewGuid(), SpotifyId = "testSpotifyIdThree", Name = "Motel" };

            dbContext.AddRange(trackOne, trackTwo, trackThree);

            // Seeding PlaylistCategory
            var playlistCategoryOne = new PlaylistCategory { CategoryId = firstCategory.Id, PlaylistId = playlistOne.Id };
            var playlistCategoryOne2 = new PlaylistCategory { CategoryId = secondCategory.Id, PlaylistId = playlistOne.Id };
            var playlistCategoryTwo = new PlaylistCategory { CategoryId = secondCategory.Id, PlaylistId = playlistTwo.Id };
            var playlistCategoryThree = new PlaylistCategory { CategoryId = thirdCategory.Id, PlaylistId = playlistThree.Id };

            dbContext.AddRange(playlistCategoryOne, playlistCategoryOne2, playlistCategoryTwo, playlistCategoryThree);

            // Seeding PlaylistTrack
            var playlistTrackOne = new PlaylistTrack { PlaylistId = playlistOne.Id, TrackId = trackOne.Id };
            var playlistTrackTwo = new PlaylistTrack { PlaylistId = playlistTwo.Id, TrackId = trackTwo.Id };
            var playlistTrackThree = new PlaylistTrack { PlaylistId = playlistThree.Id, TrackId = trackThree.Id };

            dbContext.AddRange(playlistTrackOne, playlistTrackTwo, playlistTrackThree);

            // Save all
            dbContext.SaveChanges();


            DetachAllEntities(dbContext);
            return dbContext;
        }

        private static void DetachAllEntities(TSoundDbContext context)
        {
            var changedEntriesCopy = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted ||
                            e.State == EntityState.Unchanged)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
