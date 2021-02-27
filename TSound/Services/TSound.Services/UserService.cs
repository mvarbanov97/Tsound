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

        public async Task<UserServiceModel> GetUserByEmailAsync(string email)
        {
            var user = await this.unitOfWork.Users.All().Where(x => x.Email == email).FirstOrDefaultAsync();

            return this.mapper.Map<UserServiceModel>(user);
        }
    }
}
