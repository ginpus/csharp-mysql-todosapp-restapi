using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Models.ResponseModels
{
    public class TodoItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public DateTime Date_Created { get; set; }
        public bool IsDone { get; set; }
        public Guid UserId { get; set; }
    }
}