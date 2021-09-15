using Persistence.Client;
using Persistence.Models;
using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        /*        private const string UsersTable = "users";
                private const string ApiKeysTable = "apikeys";
                private readonly ISqlClient _sqlClient;

                public UsersRepository(ISqlClient sqlClient)
                {
                    _sqlClient = sqlClient;
                }

                public async Task<int> CreateUserAsync(User user)
                {
                    var sqlInsert = @$"INSERT INTO {UsersTable} (userid, username, password, datecreated) VALUES(@userid, @username, @password, @datecreated)";
                    var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
                    {
                        userid = user.UserId,
                        username = user.UserName,
                        password = user.Password,
                        datecreated = user.DateCreated
                    }); ;
                    return await rowsAffected;
                }*/

        private readonly Dictionary<string, ApikeyReadModel> _apiKeys;

        public UsersRepository() // temporary fake entry
        {
            _apiKeys = new Dictionary<string, ApikeyReadModel> {
                {"somerandomidwhichcanbetreatedasapikey", new ApikeyReadModel
                    {
                    Id = Guid.NewGuid(),
                    Key = null,
                    UserId = default,
                    IsActive = true,
                    DateCreated = default
                    }
                },
                {"theotherapikey", new ApikeyReadModel
                    {
                    Id = Guid.NewGuid(),
                    Key = null,
                    UserId = default,
                    IsActive = true,
                    DateCreated = default
                    }
                }
            };
        }

        public ApikeyReadModel GetApiKey(string key)
        {
            Console.WriteLine(_apiKeys[key].ToString());
            Console.WriteLine(key);
            return _apiKeys[key];
        }

        /*        public async Task<int> GenerateApiKey(User user)
                {
                    var sqlInsert = @$"INSERT INTO {ApiKeysTable} (id, apikey, userid, isactive, datecreated) VALUES(@id, @apikey, @userid, @isactive, @datecreated)";
                    var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
                    {
                        id = Guid.NewGuid(),
                        apikey = user.UserName,
                        userid = user.Password,
                        isactive = true,
                        datecreated = user.DateCreated
                    }); ;
                    return await rowsAffected;
                }*/
    }
}