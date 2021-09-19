using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Dtos
{
    public class ApiKeyDto
    {
        public Guid Id { get; set; }

        public string ApiKey { get; set; }

        public Guid UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}