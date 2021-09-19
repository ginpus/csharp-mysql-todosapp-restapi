using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.ReadModels
{
    public class UserReadModel
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }

        public override string ToString()
        {
            return $"{UserId}; {UserName}; {Password}";
        }
    }
}