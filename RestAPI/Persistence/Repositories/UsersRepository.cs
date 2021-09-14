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
        private readonly Dictionary<string, ApikeyReadModel> _apiKeys;

        public UsersRepository() // temporary fake entry
        {
            _apiKeys = new Dictionary<string, ApikeyReadModel> {
                {"somerandomidwhichcanbetreatedasapikey", new ApikeyReadModel
                {
                    Id = Guid.NewGuid(),
                    Key = null,
                    UserId = default,
                    IsActive = false,
                    DateCreated = default
                } }
            };
        }

        public ApikeyReadModel GetApiKey(string key)
        {
            return _apiKeys[key];
        }
    }
}