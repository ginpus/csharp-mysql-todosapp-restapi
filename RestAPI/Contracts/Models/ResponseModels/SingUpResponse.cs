using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models.ResponseModels
{
    public class SingUpResponse
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }
    }
}