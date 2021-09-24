using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Models.RequestModels
{
    public class UpdateTodoStatusRequest
    {
        [Required]
        public bool IsDone { get; set; }
    }
}