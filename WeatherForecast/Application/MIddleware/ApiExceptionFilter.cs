using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WeatherForecast.Application.Middleware;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, "Exception caught in filter.");
        
        context.Result = new ObjectResult(new 
        { 
            error = "Incorrect data", 
            details = context.Exception.Message
        }) 
        { 
            StatusCode = 500
        };
    }
}