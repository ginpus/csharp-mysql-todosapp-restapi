using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Attributes
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    //Action filter - before entering method, it will perform the logic defined in this method
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //context.HttpContext.Request.
            var todoId = (Guid)context.ActionArguments["Id"];
            var apiKey = context.HttpContext.Request.Headers["ApiKey"].SingleOrDefault(); // this name of the header will need to be included

            if (string.IsNullOrEmpty(apiKey))
            {
                context.Result = new BadRequestObjectResult("ApiKey header is missing");

                return;
            }

            await next(); // jumps to the action OR to the other filter!
        }
    }
}