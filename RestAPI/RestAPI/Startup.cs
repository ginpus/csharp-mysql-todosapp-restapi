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
using RestAPI.Options;
using RestAPI.Services;
using RestAPI.SwaggerSettings;
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
            //Allows to receive value from JSON object "ConnectionStrings" from appsettings.json file. [] brackets allows to receive a value from inside JSON property of corresponding name
            /*            var connectionString = Configuration.GetSection("ConnectionStrings")["SqlConnectionString"];
                        var valueToChange = Configuration.GetSection("ConnectionStrings")["SomethingToBeChangedWithoutCodeChange"];*/

            services.Configure<ApiKeySettings>(Configuration.GetSection("ApiKeySettings"));
            services.Configure<SessionKeySettings>(Configuration.GetSection("SessionKeySettings"));

            services.AddControllers().AddJsonOptions(options => // required to represnet ENUM as string value (not as number)
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });

            services.AddPersistence(Configuration);

            services.AddSingleton<IApiKeysService, ApiKeysService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RestAPI", Version = "v1" });
                options.OperationFilter<AddHeaderParameter>();
            });

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