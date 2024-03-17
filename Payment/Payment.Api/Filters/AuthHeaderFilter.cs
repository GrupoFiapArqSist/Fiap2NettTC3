using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class AuthHeaderFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "API-KEY",
            In = ParameterLocation.Header,
            Description = "aPI kEY",
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "String",
                Default = new OpenApiString(string.Empty)
            }
        });
    }
}