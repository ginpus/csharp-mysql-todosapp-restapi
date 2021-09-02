using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Dtos
{
    public class AddTodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}