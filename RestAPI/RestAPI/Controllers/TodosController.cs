using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.Models;
using Persistence.Repositories;
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
        public async Task<IEnumerable<TodoItem>> GetTodos()
        {
            return await _todosRepository.GetAllAsync();
        }

        /*[HttpGet]
        [Route("comments/{commentId}")]
        public ActionResult<Comment> GetComment(Guid commentId)
        {
            var comment = _commentsRepository.Get(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost]
        [Route("comments")]
        public ActionResult<CommentResponse> AddComment([FromBody] AddCommentRequest request)
        {
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Body = request.Body
            };

            _commentsRepository.Add(comment);

            return CreatedAtAction("GetComment", new { commentId = comment.Id }, comment.MapToCommentResponse());
        }

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