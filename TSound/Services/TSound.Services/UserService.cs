using AutoMapper;
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
        private readonly IMapper mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserServiceModel>> GetAllUsersAsync()
        {
            var users = await this.unitOfWork.Users.All().ToListAsync();

            return users.Select(user => new UserServiceModel
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
            });
        }

        public async Task<UserServiceModel> GetUserByEmailAsync(string email)
        {
            var user = await this.unitOfWork.Users.All().Where(x => x.Email == email).FirstOrDefaultAsync();

            return this.mapper.Map<UserServiceModel>(user);
        }
    }
}
