using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Models;

namespace TSound.Services.Contracts
{
    public interface ISongService : IService
    {
        Task<SongServiceModel> GetSongByIdAsync(Guid songId);

        Task<IEnumerable<SongServiceModel>> GetAllSongsAsync(Guid songId);

        Task LoadSongsInDbAsync();
    }
}
