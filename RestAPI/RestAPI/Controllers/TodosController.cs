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

        [HttpPost]
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

            //could be used and should work:
            //await _todosRepository.SaveOrUpdate(todoItem);

            //return todoItem.AsDto();
            return CreatedAtAction(nameof(GetTodoItemByIdAsync), new { Id = todoItem.Id.ToString("N") }, todoItem.AsDto());
        }

        [HttpGet]
        [Route("{todoId}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemByIdAsync(Guid todoId) // Gauti konkretų TodoItem
        {
            var todo = await _todosRepository.GetTodoItemByIdAsync(todoId);

            if (todo == null)
            {
                return NotFound($"Todo item with specified id : `{todoId}` does not exist");
            }

            return Ok(todo.AsDto());
        }

        [HttpPut]
        [Route("{todoId}")]
        public async Task<ActionResult<TodoItemDto>> UpdateTodo(Guid todoId, UpdateTodoDto todo)
        {
            if (todo is null)
            {
                return BadRequest();
            }

            var todoToUpdate = await _todosRepository.GetTodoItemByIdAsync(todoId);

            if (todoToUpdate is null)
            {
                return NotFound($"Todo item with specified id : `{todoId}` does not exist");
            }

            todoToUpdate.Title = todo.Title;
            todoToUpdate.Description = todo.Description;
            todoToUpdate.Difficulty = todo.Difficulty;
            todoToUpdate.IsDone = todo.IsDone;

            await _todosRepository.SaveOrUpdate(todoToUpdate);

            return todoToUpdate.AsDto();

            /*            var todoReveresed = new UpdateTodo
                        {
                            Title = todo.Title,
                            Description = todo.Description,
                            Difficulty = todo.Difficulty,
                            IsDone = todo.IsDone
                        };

                        await _todosRepository.EditAsync(todoId, todoReveresed);

                        return todoReveresed.AsDto();*/
        }

        [HttpDelete]
        [Route("{todoId}")]
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