﻿using Persistence.Models;
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
        Task<IEnumerable<ApiKeyModel>> GetAllApiKeyAsync(Guid userid);

        Task<ApikeyReadModel> GetApiKeyAsync(string apikey);

        Task<ApiKeyModel> GenerateApiKeyAsync(Guid userId);
    }
}