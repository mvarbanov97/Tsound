using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Data.Models;

namespace TSound.Data.Seeder.Seeding
{
    public class UsersSeeder : ISeeder
    {
        private static readonly Random random = new Random();

        public async Task SeedAsync(TSoundDbContext dbContext, IServiceProvider serviceProvider)
        {
            var hasher = new PasswordHasher<User>();

            for (int i = 1; i <= 40; i++)
            {
                int gender = random.Next(0, 2);
                string image = gender == 0 ? $"/images/users/user_profile_man_{random.Next(0, 11)}.jpg" : $"/images/users/user_profile_woman_{random.Next(0, 11)}.jpg";
                string firstName = GetRandomFirstName(gender);
                string lastName = GetRandomLastName();
                string password = firstName.First().ToString().ToUpper() + firstName.Substring(1).ToLowerInvariant().Replace(" ", "") + "@1";
                string email = (firstName + lastName).ToLowerInvariant().Replace(" ", "") + "@email.com";
                DateTime dateCreated = DateTime.UtcNow.AddYears(random.Next(-5, 1)).AddMonths(random.Next(-12, 0)).AddDays(random.Next(-31, 0));

                if (dbContext.Users.Any(x => x.Email == email))
                {
                    i--;
                }
                else
                {
                    dbContext.Users.Add(new User
                    {
                        DateCreated = dateCreated,
                        DateModified = dateCreated,
                        FirstName = firstName,
                        LastName = lastName,
                        UserName = email,
                        NormalizedUserName = email.ToUpperInvariant(),
                        Email = email,
                        NormalizedEmail = email.ToUpperInvariant(),
                        EmailConfirmed = true,
                        ImageUrl = image,
                        IsDeleted = false,
                        IsBanned = false,
                        IsAdmin = false,
                        LockoutEnabled = true,
                        SecurityStamp = String.Concat(Array.ConvertAll(Guid.NewGuid().ToByteArray(), b => b.ToString("X2"))),
                    });
                    dbContext.SaveChanges();

                    User userCreated = dbContext.Users.FirstOrDefault(x => x.Email == email);
                    userCreated.PasswordHash = hasher.HashPassword(userCreated, password);
                    dbContext.SaveChanges();

                    dbContext.UserRoles.Add(new IdentityUserRole<Guid>
                    {
                        RoleId = dbContext.Roles.FirstOrDefault(x => x.Name == "User").Id,
                        UserId = userCreated.Id,
                    });
                    dbContext.SaveChanges();
                }
            }
        }

        private static string GetRandomFirstName(int gender)
        {
            string firstName;

            if (gender == 0)
            {
                firstName = listFirstNamesMale[random.Next(0, listFirstNamesMale.Count)];
            }
            else
            {
                firstName = listFirstNamesFemale[random.Next(0, listFirstNamesFemale.Count)];
            }

            return firstName;
        }

        private static string GetRandomLastName()
        {
            return listLastNames[random.Next(0, listLastNames.Count)];
        }

        private static readonly List<string> listFirstNamesMale = new List<string>()
            {
                "James",
                "Michael",
                "Robert",
                "John",
                "David",
                "William",
                "Richard",
                "Thomas",
                "Mark",
                "Charles", // 10
                "Steven",
                "Gary",
                "Joseph",
                "Donald",
                "Ronald",
                "Kenneth",
                "Paul",
                "Larry",
                "Daniel",
                "Stephen", // 20
                "Dennis",
                "Timothy",
                "Edward",
                "Jeffrey",
                "George",
                "Gregory",
                "Kevin",
                "Douglas",
                "Terry",
                "Anthony", // 30
                "Jerry",
                "Bruce",
                "Randy",
                "Brian",
                "Frank",
                "Scott",
                "Roger",
                "Raymond",
                "Peter",
                "Patrick", // 40
                "Keith",
                "Lawrence",
                "Wayne",
                "Danny",
                "Alan",
                "Gerald",
                "Ricky",
                "Carl",
                "Christopher",
                "Dale" // 50
            };

        private static readonly List<string> listFirstNamesFemale = new List<string>()
            {
                "Mary",
                "Linda",
                "Patricia",
                "Susan",
                "Deborah",
                "Barbara",
                "Debra",
                "Karen",
                "Nancy",
                "Donna", // 10
                "Cynthia",
                "Sandra",
                "Pamela",
                "Sharon",
                "Kathleen",
                "Carol",
                "Diane",
                "Brenda",
                "Cheryl",
                "Janet", // 20
                "Elizabeth",
                "Kathy",
                "Margaret",
                "Janice",
                "Carolyn",
                "Denise",
                "Judy",
                "Rebecca",
                "Joyce",
                "Teresa", // 30
                "Christine",
                "Catherine",
                "Shirley",
                "Judith",
                "Betty",
                "Beverly",
                "Lisa",
                "Laura",
                "Theresa",
                "Connie", // 40
                "Ann",
                "Gloria",
                "Julie",
                "Gail",
                "Joan",
                "Paula",
                "Peggy",
                "Cindy",
                "Martha",
                "Bonnie", // 50
            };

        private static readonly List<string> listLastNames = new List<string>()
            {
                "Smith",
                "Johnson",
                "Williams",
                "Brown",
                "Jones",
                "Garcia",
                "Miller",
                "Davis",
                "Rodriguez",
                "Martinez", // 10
                "Hernandez",
                "Lopez",
                "Gonzalez",
                "Wilson",
                "Anderson",
                "Taylor",
                "Thomas",
                "Moore",
                "Jackson",
                "Martin", // 20
                "Lee",
                "Perez",
                "Thompson",
                "White",
                "Harris",
                "Sanchez",
                "Clark",
                "Ramirez",
                "Lewis",
                "Robinson", // 30
                "Walker",
                "Young",
                "Allen",
                "King",
                "Wright",
                "Scott",
                "Torres",
                "Nguyen",
                "Hill",
                "Flores", // 40
                "Green",
                "Adams",
                "Nelson",
                "Baker",
                "Hall",
                "Rivera",
                "Campbell",
                "Mitchell",
                "Carter",
                "Roberts", // 50
            };
    }
}