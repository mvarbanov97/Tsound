using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Data.Models;

namespace TSound.Data.Seeding
{
    public class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Roles.Any())
            {
                return;
            }

            List<Role> roles = new List<Role>();

            roles.Add(new Role
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
            });

            roles.Add(new Role
            {
                Name = "UserManager",
                NormalizedName = "USERMANAGER"
            });

            roles.Add(new Role
            {
                Name = "User",
                NormalizedName = "USER"
            });

            await dbContext.AddRangeAsync(roles);
            await dbContext.SaveChangesAsync();
        }
    }
}