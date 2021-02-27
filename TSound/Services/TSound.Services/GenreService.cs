using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Extensions;
using TSound.Services.Models;

namespace TSound.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<GenreServiceModel>> GetAllGenresAsync(bool requireApiKey = false, System.Guid? apiKey = null)
        {
            if (requireApiKey)
                await this.ValidateAPIKeyAsync(apiKey, this.unitOfWork);

            var genres = this.unitOfWork.Genres.All();

            if (genres == null)
                return new List<GenreServiceModel>();

            var result = this.mapper.Map<IEnumerable<GenreServiceModel>>(genres);

            return result;
        }

        public async Task LoadGenresInDbAsync()
        {
            string[] genreIds = new string[5] { "129", "144", "116", "132", "152" };

            foreach (var id in genreIds)
            {
                var genre = await this.GetGenreFromDeezerAPIAsync(id);
                await this.unitOfWork.Genres.AddAsync(genre);
            }

            await this.unitOfWork.CompleteAsync();
        }

        private async Task<Genre> GetGenreFromDeezerAPIAsync(string genreId)
        {
            string url = $"https://api.deezer.com/genre/{genreId}";

            string jsonGenre = await this.GetJsonStreamFromUrlAsync(url);
            Genre genre = JsonConvert.DeserializeObject<Genre>(jsonGenre);

            return genre;
        }
    }
}