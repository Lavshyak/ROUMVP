using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using ROUMVP.ResultsAndErrors;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ROUMVP.Extensions;

public static class TypeExtensions
{
    public static OpenApiResponse? GetResponseForOpenApi(this Type errorEnumType, OperationFilterContext context)
    {
        if (errorEnumType.BaseType?.FullName != "System.Enum")
        {
            throw new ArgumentException();
        }
        
        var props = errorEnumType.GetFields().Skip(1).ToArray();
		
        if(props.Length == 0)
            return null;
        
        
        List<string> descriptions = new();
        foreach (var propertyInfo in props)
        {
            var descriptionAttribute = propertyInfo.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute is not null)
            {
                descriptions.Add(descriptionAttribute.Description);
            }
            else
            {
                descriptions.Add(propertyInfo.Name);
            }
        }

        var strings = descriptions.Select((d, i) => $"{i} : {d}");
        var response = new OpenApiResponse();
        response.Description = string.Join("<br/>", strings);
        response.Content.Add("application/json", new OpenApiMediaType
        {
            Schema = context.SchemaGenerator.GenerateSchema(typeof(UnprocessableHttpRequestInfo), context.SchemaRepository),
            Examples = { ["application/json"] = new OpenApiExample
            {
                Value = new OpenApiObject()
                {
                    [nameof(UnprocessableHttpRequestInfo.Code).ToLower()] = new OpenApiInteger(0),
                    [nameof(UnprocessableHttpRequestInfo.Description).ToLower()] = new OpenApiString(descriptions.First()),
                }
            } }
        });

        return response;
    }
}