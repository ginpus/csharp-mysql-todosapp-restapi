using Persistence.Models;
using RestAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI
{
    public static class Extensions
    {
        public static TodoItemDto AsDto(this TodoItem todoItem)
        {
            return new TodoItemDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                Difficulty = todoItem.Difficulty,
                IsDone = todoItem.IsDone,
                Date_Created = todoItem.Date_Created
            };
        }

        public static UpdateTodoDto AsDto(this UpdateTodo todoItem)
        {
            return new UpdateTodoDto
            {
                Title = todoItem.Title,
                Description = todoItem.Description,
                Difficulty = todoItem.Difficulty,
                IsDone = todoItem.IsDone
            };
        }
    }
}