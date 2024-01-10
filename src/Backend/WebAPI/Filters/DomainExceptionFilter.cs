using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Filters;

public class DomainExceptionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is DomainException ex)
        {
            var detail = new ValidationProblemDetails { Detail = ex.Message, Status = StatusCodes.Status400BadRequest };
            context.Result = new ObjectResult(detail) { StatusCode = detail.Status };

            context.ExceptionHandled = true;
        }
    }
}