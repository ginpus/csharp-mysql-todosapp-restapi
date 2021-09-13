using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Persistence.Client;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // required for MySQL to work with GUID type
            SqlMapper.AddTypeHandler(new MySqlGuidTypeHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            return services
                .AddSqlClient(configuration)
                .AddRepositories();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ITodosRepository, TodosRepository>();

            return services;
        }

        public static IServiceCollection AddSqlClient(this IServiceCollection services, IConfiguration configuration)
        {
            /*            var connectionStringBuilder = new MySqlConnectionStringBuilder();

                        connectionStringBuilder.Server = "localhost";
                        connectionStringBuilder.Port = 3306;
                        connectionStringBuilder.UserID = "test";
                        connectionStringBuilder.Password = "test";
                        connectionStringBuilder.Database = "todosdb";

                        var connectionString = connectionStringBuilder.GetConnectionString(true);*/

            var connectionString = configuration.GetSection("ConnectionStrings")["SqlConnectionString"];

            return services.AddTransient<ISqlClient>(_ => new SqlClient(connectionString));
        }
    }
}