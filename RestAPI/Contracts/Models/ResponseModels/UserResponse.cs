using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Models.ResponseModels
{
    public class UserResponse
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }
    }
}