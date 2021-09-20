using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.ReadModels
{
    public class SessionKeyReadModel
    {
        public Guid SessionId { get; set; }

        public string SessionKey { get; set; }

        public Guid UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
