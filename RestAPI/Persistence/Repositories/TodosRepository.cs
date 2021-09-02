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

        public Task<int> DeleteAllAsync()
        {
            var sqlDeleteAll = $"DELETE FROM {TableName}";

            var rowsAffected = _sqlClient.ExecuteAsync(sqlDeleteAll);
            return rowsAffected;
        }

        public Task<int> DeleteAsync(string id)
        {
            var sqlDelete = $"DELETE FROM {TableName} WHERE id = @id";

            var rowsAffected = _sqlClient.ExecuteAsync(sqlDelete, new
            {
                id = id
            });
            return rowsAffected;
        }

        public Task<int> EditAsync(string id, string title, string description)
        {
            var sqlUpdate = $"UPDATE {TableName} SET title = @title, description = @description where id = @id";

            var rowsAffected = _sqlClient.ExecuteAsync(sqlUpdate, new
            {
                id = id,
                title = title,
                description = description
            });
            return rowsAffected;
        }

        public Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            var sqlSelect = $"SELECT id, title, description, difficulty, date_created, isdone FROM {TableName} ORDER BY date_created desc";

            return _sqlClient.QueryAsync<TodoItem>(sqlSelect);
        }

        public Task<TodoItem> GetTodoItemByIdAsync(string id)
        {
            var sqlSelect = $"SELECT id, title, description, difficulty, date_created, isdone FROM {TableName} where id = @id ORDER BY date_created desc";

            return _sqlClient.QueryFirstOrDefaultAsync<TodoItem>(sqlSelect, new
            {
                id = id
            });
        }

        public Task<int> SaveAsync(TodoItem todoItem)
        {
            var sqlInsert = @$"INSERT INTO {TableName} (id, title, description, difficulty, date_created, isdone) VALUES(@id, @title, @description, @difficulty, @date_created, @isdone)";
            var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
            {
                id = Guid.NewGuid(),
                title = todoItem.Title,
                description = todoItem.Description,
                difficulty = todoItem.Difficulty.ToString(),
                date_created = todoItem.Date_Created,
                isdone = todoItem.IsDone
            }); ;
            return rowsAffected;
        }
    }
}