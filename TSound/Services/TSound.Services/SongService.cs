using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;

namespace TSound.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork unitOfWork;

        public SongService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
