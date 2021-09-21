using Contracts.Models.ResponseModels;
using Persistence.Models;
using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IApiKeysRepository
    {
        Task<IEnumerable<ApikeyReadModel>> GetAllApiKeyAsync(Guid userid);

        Task<ApikeyReadModel> GetApiKeyAsync(string apikey);

        Task<ApikeyReadModel> GetApiKeyByIdAsync(Guid apiKeyId);

        Task<int> SaveApiKeyAsync(ApiKeyResponse apiKey);

        Task<int> UpdateIsActive(Guid apiKeyId, bool isActive);
    }
}