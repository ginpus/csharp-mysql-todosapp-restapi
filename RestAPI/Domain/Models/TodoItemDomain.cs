using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TodoItemDomain
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public DateTime Date_Created { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Title} - {Description} - {Difficulty} - {Date_Created}";
        }
    }
}