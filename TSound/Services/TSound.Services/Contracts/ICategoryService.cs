using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface ICategoryService : IService
    {
        Task<CategoryServiceModel> GetCategoryByIdAsync(Guid genreId);

        Task<IEnumerable<CategoryServiceModel>> GetCategoryByPlaylistIdAsync(Guid id);

        Task<IEnumerable<CategoryServiceModel>> GetAllCategoriesAsync(bool requireApiKey = false, Guid? apiKey = null);

        Task LoadCategoriesInDb();

        Task<T> GetSpotifyCategoriesFromApi<T>();
    }
}
