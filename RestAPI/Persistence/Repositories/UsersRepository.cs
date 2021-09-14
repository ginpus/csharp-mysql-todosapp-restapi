using Persistence.Models.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly Dictionry<string, ApikeyReadModel> _apiKeys;

        public UserRepository()
        {
            _apiKeys = new Dictionary<string, ApikeyReadModel>
        }

        public ApikeyReadModel GetApiKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}