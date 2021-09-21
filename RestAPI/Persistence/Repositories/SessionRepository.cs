using Contracts.Models.ResponseModels;
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
    public class SessionRepository : ISessionRepository
    { 
        private const string SessionsTable = "sessionkeys";
        private readonly ISqlClient _sqlClient;

        public SessionRepository(ISqlClient sqlClient)
        {
        _sqlClient = sqlClient;
        }

        public async Task<int> SaveSessionKeyAsync(UserSessionKey sessionKey)
        {
            var sqlInsert = @$"INSERT INTO {SessionsTable} (sessionid, sessionkey, userid, isactive, datecreated, expirationdate) VALUES(@sessionid, @sessionkey, @userid, @isactive, @datecreated, @expirationdate)";
            var rowsAffected = await _sqlClient.ExecuteAsync(sqlInsert, sessionKey);

            return rowsAffected;
        }

        public async Task<SessionKeyReadModel> GetSessionKeyAsync(string sessionKey)
        {
            var sqlSelect = $"SELECT sessionid, sessionkey, userid, isactive, datecreated, expirationdate FROM {SessionsTable} WHERE sessionkey = @sessionkey";

            var foundSessionKey = await _sqlClient.QuerySingleOrDefaultAsync<SessionKeyReadModel>(sqlSelect, new
            {
                sessionkey = sessionKey
            });

            return foundSessionKey;
        }
    }
}
