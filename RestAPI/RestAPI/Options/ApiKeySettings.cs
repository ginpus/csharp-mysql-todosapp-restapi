using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Options
{
    public class ApiKeySettings
    {
        public int ExpirationTimeInMinutes { get; set; }

        public int MaxApiKeyNumber { get; set; }
    }
}
