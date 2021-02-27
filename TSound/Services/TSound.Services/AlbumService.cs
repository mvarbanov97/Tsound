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
    public class AlbumService : IAlbumService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AlbumService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<AlbumServiceModel> GetAlbumByIdAsync(Guid albumId)
        {
            var album = await this.unitOfWork.Albums.All().Include(a => a.Artist).FirstOrDefaultAsync(a => a.Id == albumId);

            if (album == null)
                throw new ArgumentNullException("Album Not Found.");

            var albumServiceModel = this.mapper.Map<AlbumServiceModel>(album);

            return albumServiceModel;
        }
    }
}
