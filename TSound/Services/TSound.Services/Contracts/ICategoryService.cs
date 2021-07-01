using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Data.Models.SpotifyDomainModels;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface ICategoryService : IService
    {
        Task<GenreServiceModel> GetCategoryByIdAsync(Guid genreId);

        Task<IEnumerable<GenreServiceModel>> GetCategoryByPlaylistIdAsync(Guid id);

        Task<IEnumerable<GenreServiceModel>> GetAllCategoriesAsync(bool requireApiKey = false, Guid? apiKey = null);

        Task LoadCategoriesInDb();

        Task<T> GetSpotifyCategoriesFromApi<T>();
    }
}
