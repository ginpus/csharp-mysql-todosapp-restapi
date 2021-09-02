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
        public async Task<IEnumerable<TodoItemDto>> GetTodos()
        {
            var todos = await _todosRepository.GetAllAsync();

            var todosDto = todos.Select(todoItem => todoItem.AsDto());

            return todosDto;
        }

        [HttpGet]
        [Route("todos/{todoId}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItemByIdAsync(string todoId) // change to Guid
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
        public async Task<ActionResult<TodoItemDto>> AddTodo(AddTodoDto todoDto)
        {
            var todoItem = new TodoItem
            {
                Id = "1",
                Title = todoDto.Title,
                Description = todoDto.Description,
                Difficulty = todoDto.Difficulty,
                IsDone = false,
                Date_Created = DateTime.Now
            };

            await _todosRepository.SaveAsync(todoItem);

            return CreatedAtAction("GetTodoItemByIdAsync", new { id = todoItem.Id }, todoItem.AsDto());
        }

        /*
        [HttpPut]
        [Route("comments/{commentId}")]
        public ActionResult<CommentResponse> UpdateComment(Guid commentId, UpdateCommentRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var commentToUpdate = _commentsRepository.Get(commentId);

            if (commentToUpdate is null)
            {
                return NotFound();
            }

            var updatedComment = _commentsRepository.Update(commentId, request.Email, request.Body);

            return updatedComment.MapToCommentResponse();
        }

        [HttpDelete]
        [Route("comments/{commentId}")]
        public IActionResult DeleteComment(Guid commentId)
        {
            var commentToDelete = _commentsRepository.Get(commentId);

            if (commentToDelete is null)
            {
                return NotFound();
            }

            _commentsRepository.Delete(commentId);

            return NoContent();
        }*/
    }
}