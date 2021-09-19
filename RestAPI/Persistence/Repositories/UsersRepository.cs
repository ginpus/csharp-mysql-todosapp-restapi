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

        public async Task<IEnumerable<UserReadModel>> GetAllUsersAsync()
        {
            var sqlSelect = $"SELECT userid, username, password, datecreated FROM {UsersTable}";

            var allUsers = await _sqlClient.QueryAsync<UserReadModel>(sqlSelect);

            return allUsers;
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
                DateCreated = DateTime.Now
            };

            var sqlInsert = @$"INSERT INTO {ApiKeysTable} (id, apikey, userid, isactive, datecreated) VALUES(@id, @apikey, @userid, @isactive, @datecreated)";
            var rowsAffected = await _sqlClient.ExecuteAsync(sqlInsert, new
            {
                id = newApiKey.Id,
                apikey = newApiKey.ApiKey,
                userid = newApiKey.UserId,
                isactive = newApiKey.IsActive,
                datecreated = newApiKey.DateCreated
            });

            return newApiKey;
        }

        public async Task<int> CreateUserAysnc(User user)
        {
            var sqlInsert = @$"INSERT INTO {UsersTable} (userid, username, password, datecreated) VALUES(@userid, @username, @password, @datecreated)";

            var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
            {
                userid = user.UserId,
                username = user.UserName,
                password = user.Password,
                datecreated = user.DateCreated
            });

            return await rowsAffected;
        }
    }
}