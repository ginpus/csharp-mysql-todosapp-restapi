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

        public async Task<int> DeleteAllAsync(Guid userid)
        {
            var sqlDeleteAll = $"DELETE FROM {TableName} WHERE userid = @userid";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlDeleteAll, new
            {
                userid = userid
            });
            return rowsAffected;
        }

        public async Task<int> DeleteAsync(Guid id, Guid userid)
        {
            var sqlDelete = $"DELETE FROM {TableName} WHERE id = @id and userid = @userid";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlDelete, new
            {
                id = id,
                userid = userid
            });
            return rowsAffected;
        }

        public async Task<int> EditAsync(Guid id, UpdateTodoWriteModel todo, Guid userid)
        {
            //var sqlUpdate = $"UPDATE {TableName} SET title = @title, description = @description, difficulty = @difficulty, isdone = @isdone  where id = @id";
            var sqlUpdate = $"UPDATE {TableName} SET title = @title, description = @description, difficulty = @difficulty  where id = @id AND userid = @userid";

            var rowsAffected = await _sqlClient.ExecuteAsync(sqlUpdate, new
            {
                id = id,
                title = todo.Title,
                description = todo.Description,
                difficulty = todo.Difficulty.ToString(),
                userid = userid
            });
            return rowsAffected;
        }

        public async Task<IEnumerable<TodoItemReadModel>> GetAllAsync()
        {
            var sqlSelect = $"SELECT id, title, description, difficulty, date_created, isdone, userid FROM {TableName} ORDER BY date_created desc";

            return await _sqlClient.QueryAsync<TodoItemReadModel>(sqlSelect);
        }

        public async Task<TodoItemReadModel> GetTodoItemByIdAsync(Guid id, Guid userid)
        {
            var sqlSelect = $"SELECT id, title, description, difficulty, date_created, isdone, userid FROM {TableName} where id = @id AND userid = @userid ORDER BY date_created desc";

            return await _sqlClient.QueryFirstOrDefaultAsync<TodoItemReadModel>(sqlSelect, new
            {
                id = id,
                userid = userid
            });
        }

        public async Task<IEnumerable<TodoItemReadModel>> GetTodoItemByUserIdAsync(Guid userid)
        {
            var sqlSelect = $"SELECT id, title, description, difficulty, date_created, isdone, userid FROM {TableName} where userid = @userid ORDER BY date_created desc";

            return await _sqlClient.QueryAsync<TodoItemReadModel>(sqlSelect, new
            {
                userid = userid
            });
        }

        public async Task<int> SaveAsync(TodoItemWriteModel todoItem)
        {
            var sqlInsert = @$"INSERT INTO {TableName} (id, title, description, difficulty, date_created, isdone, userid) VALUES(@id, @title, @description, @difficulty, @date_created, @isdone, @userid)";
            var rowsAffected = _sqlClient.ExecuteAsync(sqlInsert, new
            {
                id = todoItem.Id,
                title = todoItem.Title,
                description = todoItem.Description,
                difficulty = todoItem.Difficulty.ToString(),
                date_created = todoItem.Date_Created,
                isdone = todoItem.IsDone,
                userid = todoItem.UserId
            });
            return await rowsAffected;
        }

        public async Task<int> SaveOrUpdate(TodoItemWriteModel model)
        {
            var sql = @$"INSERT INTO {TableName} (id, title, description, difficulty, date_created, isdone, userid) VALUES(@id, @title, @description, @difficulty, @date_created, @isdone, @userid) ON DUPLICATE KEY UPDATE title = @title, description = @description, difficulty = @difficulty, isdone = @isdone, userid = @userid";

            return await _sqlClient.ExecuteAsync(sql, new
            {
                model.Id,
                model.Title,
                model.Description,
                difficulty = model.Difficulty.ToString(),
                model.Date_Created,
                model.IsDone,
                model.UserId
            });
        }
    }
}