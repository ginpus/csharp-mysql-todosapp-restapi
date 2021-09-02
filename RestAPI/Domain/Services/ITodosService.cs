using Domain.Models;
using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface ITodosService
    {
        Task<IEnumerable<TodoItemDomain>> GetAllAsync();

        Task<int> CreateAsync(TodoItem todoItem);

        Task<int> EditAsync(string id, string name, string description);

        Task<int> DeleteByIdAsync(string id);

        Task<int> ClearAllAsync();
    }
}