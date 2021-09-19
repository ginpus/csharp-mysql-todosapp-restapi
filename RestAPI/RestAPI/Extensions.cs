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
                Date_Created = todoItem.Date_Created,
                UserId = todoItem.UserId
            };
        }

        public static UpdateTodoDto AsDto(this UpdateTodo todoItem)
        {
            return new UpdateTodoDto
            {
                Title = todoItem.Title,
                Description = todoItem.Description,
                Difficulty = todoItem.Difficulty/*,
                IsDone = todoItem.IsDone*/
            };
        }

        public static UserDto AsDto(this User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Password = user.Password,
                DateCreated = user.DateCreated
            };
        }

        public static ApiKeyDto AsDto(this ApiKeyModel apiKey)
        {
            return new ApiKeyDto
            {
                Id = apiKey.Id,
                ApiKey = apiKey.ApiKey,
                UserId = apiKey.UserId,
                IsActive = apiKey.IsActive,
                DateCreated = apiKey.DateCreated
            };
        }
    }
}