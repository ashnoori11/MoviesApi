using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MoviesApi.Filters;

public class ParseBadRequest : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var result = context.Result as IStatusCodeActionResult;
        if (result is null) return;

        if(result.StatusCode == 400)
        {
            var response = new List<string>();
            var badRequestObjectResult = context.Result as BadRequestObjectResult;
            if (badRequestObjectResult is object &&  badRequestObjectResult.Value is string) {
                response.Add(badRequestObjectResult.Value?.ToString());
            }
            else if (badRequestObjectResult.Value is IEnumerable<IdentityError> errors)
            {
                foreach (var error in errors)
                {
                    response.Add(error.Description);
                }
            }
            else
            {
                foreach (var key in context.ModelState.Keys)
                {
                    foreach (var error in context.ModelState[key].Errors)
                    {
                        response.Add($"{key}: {error.ErrorMessage}");
                    }
                }
            }
            context.Result = new BadRequestObjectResult(response);
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
       
    }
}
