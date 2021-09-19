using Persistence.Client;
using Persistence.Models;
using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ApiKeysRepository : IApiKeysRepository
    {
        private const string ApiKeysTable = "apikeys";

        private readonly ISqlClient _sqlClient;

        public ApiKeysRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public async Task<IEnumerable<ApiKeyModel>> GetAllApiKeyAsync(Guid userid)
        {
            var sqlSelect = $"SELECT id, apikey, userid, isactive, datecreated, expirationdate FROM {ApiKeysTable} WHERE userid = @userid";

            var allApiKeys = await _sqlClient.QueryAsync<ApiKeyModel>(sqlSelect, new
            {
                userid = userid
            });

            return allApiKeys;
        }

        public async Task<ApikeyReadModel> GetApiKeyAsync(string apikey)
        {
            var sqlSelect = $"SELECT id, apikey, userid, isactive, datecreated, expirationdate FROM {ApiKeysTable} WHERE apikey = @apikey";

            var apiKey = await _sqlClient.QueryFirstOrDefaultAsync<ApikeyReadModel>(sqlSelect, new
            {
                apikey = apikey
            });

            return apiKey;
        }

        public async Task<ApiKeyModel> GenerateApiKeyAsync(Guid userId)
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            var generatedApiKey = Convert.ToBase64String(key);

            var newApiKey = new ApiKeyModel
            {
                Id = Guid.NewGuid(),
                ApiKey = generatedApiKey.ToString(),
                UserId = userId,
                IsActive = true,
                DateCreated = DateTime.Now,
                ExpirationDate = DateTime.Now.AddHours(4.00)
            };

            var sqlInsert = @$"INSERT INTO {ApiKeysTable} (id, apikey, userid, isactive, datecreated, expirationdate) VALUES(@id, @apikey, @userid, @isactive, @datecreated, @expiratondate)";
            var rowsAffected = await _sqlClient.ExecuteAsync(sqlInsert, new
            {
                id = newApiKey.Id,
                apikey = newApiKey.ApiKey,
                userid = newApiKey.UserId,
                isactive = newApiKey.IsActive,
                datecreated = newApiKey.DateCreated,
                expiratondate = newApiKey.ExpirationDate
            });

            return newApiKey;
        }
    }
}