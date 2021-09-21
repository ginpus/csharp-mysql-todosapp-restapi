using Contracts.Models.ResponseModels;
using Persistence.Models;
using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface ISessionRepository
    {
        Task<int> SaveSessionKeyAsync(SessionKeyWriteModel sessionKey);
        Task<SessionKeyReadModel> GetSessionKeyAsync(string sessionKey);
    }
}
