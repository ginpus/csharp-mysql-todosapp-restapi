using RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Services
{
    public interface IApiKeysService
    {
        Task<ApiKeyModel> CreateApiKey(Guid userId);

        Task<IEnumerable<ApiKeyModel>> GetApiKeys(Guid userId);

        Task<ApiKeyModel> UpdateApiKeyStatus(Guid apiKeyId, bool state, Guid userId);
    }
}
