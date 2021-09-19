using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface ITodosRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();

        Task<TodoItem> GetTodoItemByIdAsync(Guid id, Guid userid);

        Task<IEnumerable<TodoItem>> GetTodoItemByUserIdAsync(Guid userid);

        Task<int> SaveAsync(TodoItem todoItem);

        Task<int> EditAsync(Guid id, UpdateTodo todo, Guid userid);

        Task<int> DeleteAsync(Guid id, Guid userid);

        Task<int> DeleteAllAsync(Guid userid);

        Task<int> SaveOrUpdate(TodoItem model);
    }
}