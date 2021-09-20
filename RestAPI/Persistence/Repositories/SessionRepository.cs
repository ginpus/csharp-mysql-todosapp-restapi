using Contracts.Models.ResponseModels;
using Persistence.Client;
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
        //private const string UsersTable = "users";
        private readonly ISqlClient _sqlClient;

        public SessionRepository(ISqlClient sqlClient)
        {
        _sqlClient = sqlClient;
        }

        public async Task<int> SaveSessionKeyAsync(SessionKeyResponse sessionKey)
        {
            var sqlInsert = @$"INSERT INTO {SessionsTable} (sessionid, sessionkey, userid, isactive, datecreated, expirationdate) VALUES(@sessionid, @sessionkey, @userid, @isactive, @datecreated, @expirationdate)";
            var rowsAffected = await _sqlClient.ExecuteAsync(sqlInsert, sessionKey);

            return rowsAffected;
        }
    }
}
