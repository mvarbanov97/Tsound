using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models;

namespace TSound.Data.Seeding
{
    public class AdminsSeeder : ISeeder
    {
        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            var hasher = new PasswordHasher<User>();

            dbContext.Users.Add(new User
            {
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                FirstName = "Momchil",
                LastName = "Varbanov",
                UserName = "momchil.varbanov97@gmail.com",
                NormalizedUserName = "MOMCHIL.VARBANOV97@GMAIL.COM",
                Email = "momchil.varbanov97@gmail.com",
                NormalizedEmail = "MOMCHIL.VARBANOV97@GMAIL.COM",
                EmailConfirmed = true,
                ImageUrl = "/images/users/admin_profilePicture_momchilvarbanov.jpg",
                IsDeleted = false,
                IsBanned = false,
                IsAdmin = true,
                LockoutEnabled = false,
                SecurityStamp = String.Concat(Array.ConvertAll(Guid.NewGuid().ToByteArray(), b => b.ToString("X2"))),
                ApiKey = Guid.NewGuid(),
            });
            await dbContext.SaveChangesAsync();

            var admin = dbContext.Users.FirstOrDefault(x => x.Email == "momchil.varbanov97@gmail.com");
            admin.PasswordHash = hasher.HashPassword(admin, "8fipse38");
            await dbContext.SaveChangesAsync();

            dbContext.UserRoles.Add(new IdentityUserRole<Guid>
            {
                RoleId = dbContext.Roles.FirstOrDefault(x => x.Name == "Admin").Id,
                UserId = admin.Id,
            });

        }
    }
}
