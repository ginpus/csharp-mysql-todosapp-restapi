using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Dtos
{
    public class TodoItemDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public DateTime Date_Created { get; set; }
        public bool IsDone { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Title} - {Description} - {Difficulty} - {Date_Created} - Is Done?: {IsDone}";
        }
    }
}