using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.SwaggerSettings
{
    public class AddHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && descriptor.ControllerName.StartsWith("Todos"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "ApiKey",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "String"
                    }
                });
            }

            else if (descriptor != null && descriptor.ControllerName.StartsWith("ApiKey"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "SessionKey",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "String"
                    }
                });
            } else
            {
                return;
            }
        }
    }
}