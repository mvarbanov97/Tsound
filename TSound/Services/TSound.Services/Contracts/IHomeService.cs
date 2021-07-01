using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TSound.Services.Models.News;

namespace TSound.Services.Contracts
{
    public interface IHomeService : IService
    {
        Task<IEnumerable<NewsServiceModel>> GetMusicNews();
    }
}
