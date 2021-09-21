using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Dtos
{
    public class UpdateTodoRequest
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        /*        [Required]
                public bool IsDone { get; set; }*/
    }
}