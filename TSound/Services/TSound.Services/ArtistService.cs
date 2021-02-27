using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Models;

namespace TSound.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ArtistService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ArtistServiceModel> GetArtistByIdAsync(Guid artistId)
        {
            var artist = await this.unitOfWork.Artists.All().FirstOrDefaultAsync(x => x.Id == artistId);

            if (artist == null)
                throw new ArgumentNullException("Artist Not Found.");

            var artistServiceModel = this.mapper.Map<ArtistServiceModel>(artist);

            return artistServiceModel;

        }
    }
}
