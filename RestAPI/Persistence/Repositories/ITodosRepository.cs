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
        Task<IEnumerable<TodoItemReadModel>> GetAllAsync();

        Task<TodoItemReadModel> GetTodoItemByIdAsync(Guid id, Guid userid);

        Task<IEnumerable<TodoItemReadModel>> GetTodoItemByUserIdAsync(Guid userid);

        Task<int> SaveAsync(TodoItemWriteModel todoItem);

        Task<int> EditAsync(Guid id, UpdateTodo todo, Guid userid);

        Task<int> DeleteAsync(Guid id, Guid userid);

        Task<int> DeleteAllAsync(Guid userid);

        Task<int> SaveOrUpdate(TodoItemWriteModel model);
    }
}