using Domain.Models;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    internal class TodosService : ITodosService
    {
        private readonly ITodosRepository _todosRepository;

        public TodosService(ITodosRepository todosRepository)
        {
            _todosRepository = todosRepository;
        }

        public async Task<int> ClearAllAsync()
        {
            var todosDeleteAll = _todosRepository.DeleteAllAsync();

            return await todosDeleteAll;
        }

        public async Task<int> CreateAsync(TodoItem todoItem)
        {
            var todoCreate = _todosRepository.SaveAsync(todoItem);

            //await Task.WhenAll(todoCreate);

            return await todoCreate;
        }

        public async Task<int> DeleteByIdAsync(int id)
        {
            var todoDelete = _todosRepository.DeleteAsync(id);

            //await Task.WhenAll(todoDelete);

            return await todoDelete;
        }

        public async Task<int> EditAsync(int id, string name, string description)
        {
            var todoEdit = _todosRepository.EditAsync(id, name, description);

            //await Task.WhenAll(todoEdit);

            return await todoEdit;
        }

        public async Task<IEnumerable<TodoItemDomain>> GetAllAsync()
        {
            var todos = await _todosRepository.GetAllAsync(); //await is needed as we do the remaping of model
            return todos.Select(todo => new TodoItemDomain
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Difficulty = todo.Difficulty,
                Date_Created = todo.Date_Created
            });
        }
    }
}