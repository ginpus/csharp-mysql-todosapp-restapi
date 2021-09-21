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
    public class SessionKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var key = context.HttpContext.Request.Headers["SessionKey"].SingleOrDefault(); // this name of the header will need to be included

            if (string.IsNullOrEmpty(key))
            {
                context.Result = new BadRequestObjectResult("SessionKey header is missing");

                return;
            }

            var sessionKeysRepository = context.HttpContext.RequestServices.GetService<ISessionRepository>();

            var sessionKey = await sessionKeysRepository.GetSessionKeyAsync(key);

            if (sessionKey is null)
            {
                context.Result = new NotFoundObjectResult("SessionKey is not found");

                return;
            }

            if (!sessionKey.IsActive)
            {
                context.Result = new BadRequestObjectResult("sessionKey is inactive");

                return;
            }

            if (sessionKey.ExpirationDate <= DateTime.Now)
            {
                context.Result = new BadRequestObjectResult("sessionKey expired");

                return;
            }

            context.HttpContext.Items.Add("userId", sessionKey.UserId);

            await next(); // jumps to the action OR to the other filter!
        }
    }
}