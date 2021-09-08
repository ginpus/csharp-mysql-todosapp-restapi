using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Models.ReadModels
{
    public class UpdateTodoStatus
    {
        [Required]
        public bool IsDone { get; set; }
    }
}