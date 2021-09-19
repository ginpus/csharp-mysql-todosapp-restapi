using Persistence.Models;
using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns></returns>
        Task<UserReadModel> GetUserAsync(string username);

        Task<UserReadModel> GetUserAsync(string username, string password);

        Task<int> CreateUserAysnc(User user);

        Task<IEnumerable<UserReadModel>> GetAllUsersAsync();
    }
}