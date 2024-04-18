using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ROUMVP.ResultsAndErrors;

public class ResultOrUnprocessableFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult objectResult)
        {
            return;
        }
        
        if (objectResult.Value is not IResultOrUnprocessable resultOrUnprocessable)
        {
            return;
        }
        
        if(resultOrUnprocessable.IsSuccess is false)
            return;

        if (resultOrUnprocessable.ResultObject is IActionResult actionResult)
        {
            context.Result = actionResult;
        }
        else
        {
            var objectResultForReplace = new ObjectResult(resultOrUnprocessable.ResultObject);
            context.Result = objectResultForReplace;
        }
    }
}