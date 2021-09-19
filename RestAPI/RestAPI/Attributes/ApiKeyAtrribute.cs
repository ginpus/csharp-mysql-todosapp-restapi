using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace RestAPI.Attributes
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    //Action filter - before entering method, it will perform the logic defined in this method
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var key = context.HttpContext.Request.Headers["ApiKey"].SingleOrDefault(); // this name of the header will need to be included

            if (string.IsNullOrEmpty(key))
            {
                context.Result = new BadRequestObjectResult("ApiKey header is missing");

                return;
            }

            var usersRepository = context.HttpContext.RequestServices.GetService<IUsersRepository>();

            var apiKey = await usersRepository.GetApiKeyAsync(key);

            if (apiKey is null)
            {
                context.Result = new NotFoundObjectResult("Apikey is not found");

                return;
            }

            if (!apiKey.IsActive || apiKey.ExpirationDate < DateTime.Now)
            {
                context.Result = new BadRequestObjectResult("Apikey expired");

                return;
            }

            context.HttpContext.Items.Add("userId", apiKey.UserId);

            await next(); // jumps to the action OR to the other filter!
        }
    }
}