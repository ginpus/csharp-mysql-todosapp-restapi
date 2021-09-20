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

        public async Task<UserReadModel> GetUserAsync(string username)
        {
            var sqlSelect = $"SELECT userid, username, password, datecreated FROM {UsersTable} where username = @username";

            var user = await _sqlClient.QuerySingleOrDefaultAsync<UserReadModel>(sqlSelect, new
            {
                username = username
            });

            return user;
        }

        public async Task<UserReadModel> GetUserAsync(string username, string password)
        {
            var sqlSelect = $"SELECT userid, username, password, datecreated FROM {UsersTable} where username = @username AND password = @password";

            var user = await _sqlClient.QuerySingleOrDefaultAsync<UserReadModel>(sqlSelect, new
            {
                username = username,
                password = password
            });

            return user;
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

        public async Task<IEnumerable<UserReadModel>> GetAllUsersAsync()
        {
            var sqlSelect = $"SELECT userid, username, password, datecreated FROM {UsersTable}";

            var allUsers = await _sqlClient.QueryAsync<UserReadModel>(sqlSelect);

            return allUsers;
        }
    }
}