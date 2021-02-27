using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface IGenreService : IService
    {
        Task<GenreServiceModel> GetGenreByIdAsync(Guid genreId);

        Task<IEnumerable<GenreServiceModel>> GetAllGenresAsync(bool requireApiKey = false, Guid? apiKey = null);
    }
}
