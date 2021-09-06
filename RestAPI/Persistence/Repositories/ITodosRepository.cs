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

        Task<TodoItem> GetTodoItemByIdAsync(Guid id);

        Task<int> SaveAsync(TodoItem todoItem);

        Task<int> EditAsync(Guid id, UpdateTodo todo);

        Task<int> DeleteAsync(Guid id);

        Task<int> DeleteAllAsync();
    }
}