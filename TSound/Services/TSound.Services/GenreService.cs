using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TSound.Data.Models;
using TSound.Data.UnitOfWork;
using TSound.Services.Contracts;
using TSound.Services.Extensions;

namespace TSound.Services
{
    public class GenreService: IGenreService
    {
        private readonly IUnitOfWork unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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

            string jsonGenre = await this.GetJsonStreamFromDeezerAsync(url);
            Genre genre = JsonConvert.DeserializeObject<Genre>(jsonGenre);

            return genre;
        }

    }
}
