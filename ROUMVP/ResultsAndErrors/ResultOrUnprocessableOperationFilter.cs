using Microsoft.OpenApi.Models;
using ROUMVP.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ROUMVP.ResultsAndErrors;

public class ResultOrUnprocessableOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var rt = context.MethodInfo.ReturnType;

        if (rt.Name == typeof(Task<>).Name)
        {
            rt = rt.GenericTypeArguments.First();
        }
        
        var rightType =typeof(ResultOrUnprocessable<,>);
        if (rt.Name != rightType.Name || rt.Namespace != rightType.Namespace)
        {
            return;
        }
        
        operation.Responses.Clear();
        
        var resultType = rt.GenericTypeArguments.First();
        
        var response200 = new OpenApiResponse();
        response200.Description = "Success";
        response200.Content.Add("application/json", new OpenApiMediaType
        {
            Schema = context.SchemaGenerator.GenerateSchema(resultType, context.SchemaRepository)
        });
        
        operation.Responses.Add("200", response200);
        
        // error
        var errorEnumType = rt.GenericTypeArguments.Last();

        var response422 = errorEnumType.GetResponseForOpenApi(context);
        
        if(response422 is null)
            return;
        
        operation.Responses.Add("422", response422);
    }
}