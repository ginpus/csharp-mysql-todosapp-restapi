using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.ReadModels
{
    public class ApikeyReadModel
    {
        public Guid Id { get; set; }

        public string Key { get; set; }

        public Guid UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; Key: {Key}; UserId: {UserId}; IsActive: {IsActive}; DateCreated: {DateCreated}";
        }
    }
}