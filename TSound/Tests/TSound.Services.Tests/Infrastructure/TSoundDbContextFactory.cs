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
            dbContext.SaveChanges();

            // Seeding categories
            var firstCategory = new Category { Id = Guid.NewGuid(), Name = "Hip Hop", SpotifyId = "hiphop" };
            var secondCategory = new Category { Id = Guid.NewGuid(), Name = "Chill", SpotifyId = "chill" };
            var thirdCategory = new Category { Id = Guid.NewGuid(), Name = "Rock", SpotifyId = "rock" };

            dbContext.AddRange(firstCategory, secondCategory, thirdCategory);
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
