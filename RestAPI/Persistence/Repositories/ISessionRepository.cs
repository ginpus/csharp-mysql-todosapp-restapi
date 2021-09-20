﻿using Contracts.Models.ResponseModels;
using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    interface ISessionRepository
    {
        Task<int> SaveSessionKeyAsync(SessionKeyResponse sessionKey);
    }
}
