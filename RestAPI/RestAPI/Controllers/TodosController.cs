using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using Persistence.Repositories;
using RestAPI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodosRepository _todosRepository;

        public TodosController(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        [HttpGet]
        [Route("todos")]
        public async Task<IEnumerable<TodoItemDto>> GetTodos() // Gauti visus TodoItems
        {
            var todos = (await _todosRepository.GetAllAsync())
                        .Select(todoItem => todoItem.AsDto());

            return todos;

            ////same as:
            //var todos = await _todosRepository.GetAllAsync();
            //var todosDto = todos.Select(todoItem => todoItem.AsDto());
            //return todosDto;
        }

        [HttpGet]
        [Route("todos/{todoId}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemByIdAsync(Guid todoId) // Gauti konkretų TodoItem
        {
            var todo = await _todosRepository.GetTodoItemByIdAsync(todoId);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo.AsDto());
        }

        [HttpPost]
        [Route("todos")]
        public async Task<ActionResult<TodoItemDto>> AddTodo(AddTodoDto todoDto) // Pridėti TodoItem
        {
            var todoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = todoDto.Title,
                Description = todoDto.Description,
                Difficulty = todoDto.Difficulty,
                IsDone = false,
                Date_Created = DateTime.Now
            };

            await _todosRepository.SaveAsync(todoItem);

            return todoItem.AsDto();
            //return CreatedAtAction(nameof(GetTodoItemByIdAsync), new { id = todoItem.Id }, todoItem.AsDto()); // Returns exception "No route matches the supplied values."
        }

        [HttpPut]
        [Route("todos/{todoId}")]
        public async Task<ActionResult<UpdateTodoDto>> UpdateTodo(Guid todoId, UpdateTodoDto todo)
        {
            if (todo is null)
            {
                return BadRequest();
            }

            var todoToUpdate = _todosRepository.GetTodoItemByIdAsync(todoId);

            if (todoToUpdate is null)
            {
                return NotFound();
            }

            var todoReveresed = new UpdateTodo
            {
                Title = todo.Title,
                Description = todo.Description,
                Difficulty = todo.Difficulty,
                IsDone = todo.IsDone
            };

            var updatedTodo = await _todosRepository.EditAsync(todoId, todoReveresed);

            return todoReveresed.AsDto();
        }

        [HttpDelete]
        [Route("todos/{todoId}")]
        public async Task<IActionResult> DeleteTodo(Guid todoId)
        {
            var todoToUpdate = _todosRepository.GetTodoItemByIdAsync(todoId);

            if (todoToUpdate is null)
            {
                return NotFound();
            }

            await _todosRepository.DeleteAsync(todoId);

            return NoContent();
        }
    }
}