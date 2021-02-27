using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Models;

namespace TSound.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<UserServiceModel> GetUserByEmailAsync(string email)
        {
            var user = await this.unitOfWork.Users.All().Where(x => x.Email == email).FirstOrDefaultAsync();

            return new UserServiceModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateCreated = user.DateCreated,
                DateModified = user.DateModified,
                Image = user.ImageUrl,
                IsAdmin = user.IsAdmin,
                IsBanned = user.IsBanned,
                IsDeleted = user.IsDeleted,
            };

        }

        public async Task<bool> UpdateUserImageAsync(Guid id)
        {
            var user = await this.unitOfWork.Users.All().FirstOrDefaultAsync(x => x.Id == id);

            try
            {
                //this.ValidateUser(user);
            }
            catch (Exception)
            {
                return false;
            }

            user.ImageUrl = $"/images/users/{id}.jpg";
            await unitOfWork.CompleteAsync();
            return true;


        }
    }
}
