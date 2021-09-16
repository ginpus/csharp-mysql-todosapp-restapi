using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
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

        public TodosController(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
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
        public async Task<ActionResult<TodoItemDto>> UpdateTodo(Guid todoId, UpdateTodoDto todo)
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
        public async Task<ActionResult<TodoItemDto>> UpdateTodoStatus(Guid todoId, UpdateTodoStatusDto todo)
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
        public async Task<IActionResult> DeleteTodo(Guid todoId)
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