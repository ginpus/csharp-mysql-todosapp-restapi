using Contracts.Models.ResponseModels;
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

            var apiKey = await _sqlClient.QuerySingleOrDefaultAsync<ApikeyReadModel>(sqlSelect, new
            {
                apikey = apikey
            });

            return apiKey;
        }

        public async Task<ApikeyReadModel> GetApiKeyByIdAsync(Guid apiKeyId)
        {
            var sqlSelect = $"SELECT id, apikey, userid, isactive, datecreated, expirationdate FROM {ApiKeysTable} WHERE id = @apiKeyId";

            var apiKey = await _sqlClient.QuerySingleOrDefaultAsync<ApikeyReadModel>(sqlSelect, new
            {
                apiKeyId = apiKeyId
            });

            return apiKey;
        }

        public async Task<int> SaveApiKeyAsync(ApiKeyResponse apiKey)
        {
            var sqlInsert = @$"INSERT INTO {ApiKeysTable} (id, apikey, userid, isactive, datecreated, expirationdate) VALUES(@id, @apikey, @userid, @isactive, @datecreated, @expirationdate)";
            var rowsAffected = await _sqlClient.ExecuteAsync(sqlInsert, apiKey);

            return rowsAffected;
        }

        public async Task<int> UpdateIsActive(Guid apiKeyId, bool isActive)
        {
            var sqlUpdate = $"UPDATE {ApiKeysTable} SET isactive = @isactive WHERE id = @id ";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlUpdate, new
            {
                id = apiKeyId,
                isactive = isActive
            });

            return rowsAffected;
        }
    }
}