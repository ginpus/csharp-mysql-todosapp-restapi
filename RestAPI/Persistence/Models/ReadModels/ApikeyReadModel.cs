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

        public string ApiKey { get; set; }

        public Guid UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ExpirationDate { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}; ApiKey: {ApiKey}; UserId: {UserId}; IsActive: {IsActive}; DateCreated: {DateCreated}";
        }
    }
}