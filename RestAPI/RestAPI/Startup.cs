using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using Persistence;
using Persistence.Client;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*            var connectionStringBuilder = new MySqlConnectionStringBuilder();

                        connectionStringBuilder.Server = "localhost";
                        connectionStringBuilder.Port = 3306;
                        connectionStringBuilder.UserID = "test";
                        connectionStringBuilder.Password = "test";
                        connectionStringBuilder.Database = "todosdb";

                        var connectionString = connectionStringBuilder.GetConnectionString(true);

                        services.AddTransient<ISqlClient>(_ => new SqlClient(connectionString));*/

            //services.AddSingleton<ITodosRepository, TodosRepository>();

            //SAME SIMPLIFIED
            //services.AddSqlClient();

            //services.AddRepositories();

            //SAME EVEN MORE SIMPLIFIED (PERSITENCE SERVICE EXTENSION)
            services.AddPersistence();

            services.AddControllers().AddJsonOptions(options => // required to represnet ENUM as string value (not as number)
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestAPI", Version = "v1" }); });

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestAPI v1"));
            }

            app.UseHttpsRedirection();

            // request.Route - "localhost:5001/weatherforecast"
            // request.body
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin());

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}