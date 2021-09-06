using Persistence.Client;
using Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class TodosRepository : ITodosRepository
    {
        private const string TableName = "todos";
        private readonly ISqlClient _sqlClient;

        public TodosRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public async Task<int> DeleteAllAsync()
        {
            var sqlDeleteAll = $"DELETE FROM {TableName}";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlDeleteAll);
            return rowsAffected;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var sqlDelete = $"DELETE FROM {TableName} WHERE id = @id";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlDelete, new
            {
                id = id
            });
            return rowsAffected;
        }

        public async Task<int> EditAsync(Guid id, UpdateTodo todo)
        {
            var sqlUpdate = $"UPDATE {TableName} SET title = @title, description = @description, difficulty = @difficulty, isdone = @isdone  where id = @id";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlUpdate, new
            {
                id = id,
                title = todo.Title,
                description = todo.Description,
                difficulty = todo.Difficulty.ToString(),
                isdone = todo.IsDone
            });
            return rowsAffected;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            var sqlSelect = $"SELECT id, title, description, difficulty, date_created, isdone FROM {TableName} ORDER BY date_created desc";

            return await _sqlClient.QueryAsync<TodoItem>(sqlSelect);
        }

        public async Task<TodoItem> GetTodoItemByIdAsync(Guid id)
        {
            var sqlSelect = $"SELECT id, title, description, difficulty, date_created, isdone FROM {TableName} where id = @id ORDER BY date_created desc";

            return await _sqlClient.QueryFirstOrDefaultAsync<TodoItem>(sqlSelect, new
            {
                id = id
            });
        }

        public async Task<int> SaveAsync(TodoItem todoItem)
        {
            var sqlInsert = @$"INSERT INTO {TableName} (id, title, description, difficulty, date_created, isdone) VALUES(@id, @title, @description, @difficulty, @date_created, @isdone)";
            var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
            {
                id = todoItem.Id, // should be Guid
                title = todoItem.Title,
                description = todoItem.Description,
                difficulty = todoItem.Difficulty.ToString(),
                date_created = todoItem.Date_Created,
                isdone = todoItem.IsDone
            }); ;
            return await rowsAffected;
        }
    }
}