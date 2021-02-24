using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;

namespace TSound.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
