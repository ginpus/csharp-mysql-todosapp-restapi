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
        Task<IEnumerable<ApikeyReadModel>> GetAllApiKeyAsync();

        Task<ApikeyReadModel> GetApiKeyAsync(string apikey);

        Task<int> GenerateApiKey(User user);
    }
}