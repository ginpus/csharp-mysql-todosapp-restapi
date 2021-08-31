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

        Task<int> SaveAsync(TodoItem todoItem);

        Task<int> EditAsync(int id, string title, string description);

        Task<int> DeleteAsync(int id);

        Task<int> DeleteAllAsync();
    }
}