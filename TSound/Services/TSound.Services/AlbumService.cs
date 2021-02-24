using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;

namespace TSound.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IUnitOfWork unitOfWork;

        public AlbumService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
