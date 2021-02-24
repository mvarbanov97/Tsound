using System;
using System.Collections.Generic;
using System.Text;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;

namespace TSound.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlaylistService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}
