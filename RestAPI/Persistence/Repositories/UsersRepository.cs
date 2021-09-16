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
    public class UsersRepository : IUsersRepository
    {
        private const string UsersTable = "users";
        private const string ApiKeysTable = "apikeys";

        private readonly ISqlClient _sqlClient;

        public UsersRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public async Task<IEnumerable<ApikeyReadModel>> GetAllApiKeyAsync()
        {
            var sqlSelect = $"SELECT id, apikey, userid, isactive, datecreated FROM {ApiKeysTable}";

            var allApiKeys = await _sqlClient.QueryAsync<ApikeyReadModel>(sqlSelect);

            return allApiKeys;
        }

        public async Task<ApikeyReadModel> GetApiKeyAsync(string apikey)
        {
            var sqlSelect = $"SELECT id, apikey, userid, isactive, datecreated FROM {ApiKeysTable} WHERE apikey = @apikey";

            var apiKey = await _sqlClient.QueryFirstOrDefaultAsync<ApikeyReadModel>(sqlSelect, new
            {
                apikey = apikey
            });

            return apiKey;
        }

        public async Task<int> GenerateApiKey(User user)
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            var newApiKey = Convert.ToBase64String(key);

            var sqlInsert = @$"INSERT INTO {ApiKeysTable} (id, apikey, userid, isactive, datecreated) VALUES(@id, @apikey, @userid, @isactive, @datecreated)";
            var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
            {
                id = Guid.NewGuid(),
                apikey = newApiKey,
                userid = user.UserId,
                isactive = true,
                datecreated = user.DateCreated
            });

            return await rowsAffected;
        }
    }
}