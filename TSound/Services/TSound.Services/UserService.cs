using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TSound.Data.UnitOfWork;
using TSound.Plugin.Spotify.WebApi.SpotifyModels;
using TSound.Services.Contracts;
using TSound.Services.Models;

namespace TSound.Services
{
    public class UserService : IUserService
    {
        public const int PageCountSize = 12;

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper, IHttpClientFactory clientFactory)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<UserServiceModel> GetUserByIdAsync(Guid userId)
        {
            var user = await this.unitOfWork.Users.All()
                .Include(u => u.Playlists)
                .Where(x => x.Id == userId).FirstOrDefaultAsync();

            return this.mapper.Map<UserServiceModel>(user);
        }

        public async Task<IEnumerable<UserServiceModel>> GetAllUsersAsync(bool isAdmin = false, int page = 1)
        {
            var users = await this.unitOfWork.Users.All()
                .Where(x => x.IsDeleted == false)
                .Include(u => u.Playlists)
                .ToListAsync();

            if (!isAdmin)
            {
                users = users.Where(x => x.IsBanned == false).ToList();
            }

            var usersByPage = users
                .Skip((page - 1) * PageCountSize)
                .Take(PageCountSize);

            return this.mapper.Map<IEnumerable<UserServiceModel>>(usersByPage);
        }

        public async Task<UserServiceModel> GetUserByEmailAsync(string email)
        {
            var user = await this.unitOfWork.Users.All().Where(x => x.Email == email).FirstOrDefaultAsync();

            return this.mapper.Map<UserServiceModel>(user);
        }

        /// <summary>
        /// An async method that receives the id of a user and swaps its ban status.
        /// </summary>
        /// <param name="id">A Guid which is the identity of the user in the database.</param>
        public async Task SwapUserBanStatusByIdAsync(Guid id)
        {
            var userInDb = await this.unitOfWork.Users.All().FirstOrDefaultAsync(x => x.Id == id);

            userInDb.IsBanned = !userInDb.IsBanned;
            userInDb.DateModified = DateTime.UtcNow;

            await this.unitOfWork.CompleteAsync();
        }

        public int GetTotalUsersCount()
        {
            return this.unitOfWork.Users.All().Where(x => !x.IsDeleted).Count();
        }

        public int GetPageCountSizing()
        {
            int pageCountSize = PageCountSize;

            return pageCountSize;
        }
    }
}
