using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;

namespace TSound.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork unitOfWork;

        public ArtistService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
