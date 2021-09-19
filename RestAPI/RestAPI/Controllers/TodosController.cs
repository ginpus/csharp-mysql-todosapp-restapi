using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using Persistence.Models.ReadModels;
using Persistence.Repositories;
using RestAPI.Attributes;
using RestAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController] // adds some functionality which we do not need to take care in code
    [Route("todos")] // all methods will use this route as a base
    public class TodosController : ControllerBase
    {
        private readonly ITodosRepository _todosRepository;
        private readonly IUsersRepository _userRepository;
        private readonly IApiKeysRepository _apiKeysRepository;

        public TodosController(ITodosRepository todosRepository, IUsersRepository userRepository, IApiKeysRepository apiKeysRepository)
        {
            _todosRepository = todosRepository;
            _userRepository = userRepository;
            _apiKeysRepository = apiKeysRepository;
        }

        [HttpGet]
        [ApiKey]
        public async Task<IEnumerable<TodoItemDto>> GetTodos() // Gauti visus TodoItems
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var todos = (await _todosRepository.GetTodoItemByUserIdAsync(userId))
                        .Select(todoItem => todoItem.AsDto());

            return todos;
        }

        [HttpGet]
        [ApiKey]
        [Route("apikey")]
        public async Task<IEnumerable<ApiKeyDto>> GetAllApiKeysAsync() // Useris gali peržiūrėti savo ApiKeys
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var apiKeys = (await _apiKeysRepository.GetAllApiKeyAsync(userId))
                        .Select(apiKey => apiKey.AsDto());

            return apiKeys;
        }

        [HttpPost]
        [ApiKey]
        public async Task<ActionResult<TodoItemDto>> AddTodo(AddTodoDto todoDto) // Pridėti TodoItem
        {
            var userId = (Guid)HttpContext.Items["userId"];

            Console.WriteLine(userId);

            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = todoDto.Title,
                Description = todoDto.Description,
                Difficulty = todoDto.Difficulty,
                IsDone = false,
                Date_Created = DateTime.Now,
                UserId = userId
            };

            await _todosRepository.SaveAsync(todoItem);

            //could be used and should work:
            //await _todosRepository.SaveOrUpdate(todoItem);

            return todoItem.AsDto();
            //return CreatedAtAction(nameof(GetTodoItemByIdAsync), new { Id = todoItem.Id }, todoItem.AsDto());
        }

        [HttpPost]
        [Route("usercreate")]
        //[ApiKey] // as this is new user, API key does not exist at all
        public async Task<ActionResult<UserDto>> CreateUser(AddUserDto user) // Useris gali susikurti account'ą
        {
            //var userId = (Guid)HttpContext.Items["userId"];

            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserName = user.UserName,
                Password = user.Password,
                DateCreated = DateTime.Now
            };

            await _userRepository.CreateUserAysnc(newUser);

            return newUser.AsDto();
            //return CreatedAtAction(nameof(GetTodoItemByIdAsync), new { Id = todoItem.Id }, todoItem.AsDto());
        }

        [HttpPost]
        [Route("generateapikey")]
        //[ApiKey] // does not make sense, as new user will not have any API yet
        public async Task<ActionResult<ApiKeyDto>> GenerateApiKey(ReadUserDto user) // Useris gali susigeneruoti ApiKey
        {
            //var userId = (Guid)HttpContext.Items["userId"];

            var allUsersFromDb = await _userRepository.GetAllUsersAsync();
            foreach (var selectedUser in allUsersFromDb)
            {
                Console.WriteLine(selectedUser);
            };

            var userFromDb = allUsersFromDb.FirstOrDefault(userInDb => user.UserName == userInDb.UserName & user.Password == userInDb.Password);

            Console.WriteLine($"Selected user: {userFromDb}");

            if (userFromDb is not null)
            {
                var apiKey = await _apiKeysRepository.GenerateApiKeyAsync(userFromDb.UserId);
                return apiKey.AsDto();
            }
            else
            {
                Console.WriteLine("Wrong username or password.");
                var apiKey = new ApiKeyModel
                {
                    Id = default,
                    ApiKey = "noapikey",
                    UserId = default,
                    DateCreated = default,
                    IsActive = false,
                    ExpirationDate = default
                };
                return apiKey.AsDto();
            }
            //return CreatedAtAction(nameof(GetTodoItemByIdAsync), new { Id = todoItem.Id }, todoItem.AsDto());
        }

        [HttpGet]
        [Route("{todoId}")]
        [ApiKey]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemByIdAsync(Guid todoId) // Gauti konkretų TodoItem
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var todo = await _todosRepository.GetTodoItemByIdAsync(todoId, userId);

            if (todo == null)
            {
                return NotFound($"Todo item with specified id : `{todoId}` does not exist");
            }

            return Ok(todo.AsDto());
        }

        [HttpPut]
        [Route("{todoId}")]
        [ApiKey]
        public async Task<ActionResult<TodoItemDto>> UpdateTodo(Guid todoId, UpdateTodoDto todo) // Pakeisti todo itemo savybes
        {
            var userId = (Guid)HttpContext.Items["userId"];
            if (todo is null)
            {
                return BadRequest();
            }

            var todoToUpdate = await _todosRepository.GetTodoItemByIdAsync(todoId, userId);

            if (todoToUpdate is null)
            {
                return NotFound($"Todo item with specified id : `{todoId}` does not exist");
            }

            todoToUpdate.Title = todo.Title;
            todoToUpdate.Description = todo.Description;
            todoToUpdate.Difficulty = todo.Difficulty;

            await _todosRepository.SaveOrUpdate(todoToUpdate);

            return todoToUpdate.AsDto();
        }

        [HttpPatch]
        [Route("{todoId}/status")]
        [ApiKey]
        public async Task<ActionResult<TodoItemDto>> UpdateTodoStatus(Guid todoId, UpdateTodoStatusDto todo) // Pakeisti statusa todo itemo
        {
            var userId = (Guid)HttpContext.Items["userId"];

            if (todo is null)
            {
                return BadRequest();
            }

            var todoToUpdate = await _todosRepository.GetTodoItemByIdAsync(todoId, userId);

            if (todoToUpdate is null)
            {
                return NotFound($"Todo item with specified id : `{todoId}` does not exist");
            }

            todoToUpdate.IsDone = todo.IsDone;

            await _todosRepository.SaveOrUpdate(todoToUpdate);

            return todoToUpdate.AsDto();
        }

        [HttpDelete]
        [Route("{todoId}")]
        [ApiKey]
        public async Task<IActionResult> DeleteTodo(Guid todoId) // Istrinti todo
        {
            var userId = (Guid)HttpContext.Items["userId"];

            var todoToUpdate = _todosRepository.GetTodoItemByIdAsync(todoId, userId);

            if (todoToUpdate is null)
            {
                return NotFound();
            }

            await _todosRepository.DeleteAsync(todoId, userId);

            return NoContent();
        }
    }
}