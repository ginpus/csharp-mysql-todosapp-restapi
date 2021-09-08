using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Dtos
{
    public class UpdateTodoStatusDto
    {
        [Required]
        public bool IsDone { get; set; }
    }
}