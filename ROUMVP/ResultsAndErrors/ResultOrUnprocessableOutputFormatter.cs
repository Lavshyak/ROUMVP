using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ROUMVP.ResultsAndErrors;

public class ResultOrUnprocessableOutputFormatter : IOutputFormatter
{

    private static readonly PropertyInfo ObjectPropertyInfo;
    private static readonly PropertyInfo ObjectTypePropertyInfo;
    static ResultOrUnprocessableOutputFormatter()
    {
        var t = typeof(OutputFormatterCanWriteContext);
        ObjectPropertyInfo = t.GetProperty("Object") ?? throw new MissingMemberException();
        ObjectTypePropertyInfo = t.GetProperty("ObjectType") ?? throw new MissingMemberException();
    }
    
    public bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
        if (context.Object is not IResultOrUnprocessable resultOrUnprocessable)
        {
            return false;
        }

        if (resultOrUnprocessable.IsSuccess)
        {
            //here i replace ResultOrUnprocessable<,> with FileResult
            ObjectPropertyInfo.SetValue(context, resultOrUnprocessable.ResultObject);
            ObjectTypePropertyInfo.SetValue(context, resultOrUnprocessable.ResultObject?.GetType());
            
            return false;
        }

        return true;
    }

    public async Task WriteAsync(OutputFormatterWriteContext context)
    {
        if (context.Object is not IResultOrUnprocessable resultOrUnprocessable)
        {
            throw new InvalidCastException();
        }
        
        var response = context.HttpContext.Response;
        
        if (!resultOrUnprocessable.IsSuccess)
        {
            var serialized = JsonSerializer.Serialize(resultOrUnprocessable.UnprocessableInfoObject);
            response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            response.ContentType = MediaTypeNames.Application.Json;
            await response.WriteAsync(serialized);
        }
        else
        {
            throw new UnreachableException();
        }
    }
}