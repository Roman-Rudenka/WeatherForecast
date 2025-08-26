using Newtonsoft.Json;
using WeatherForecast.Presentation.Common;

namespace WeatherForecast.Presentation.Middleware;

public class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApiBaseException ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex.StatusCode, ex.Message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, 500, "Server error");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
    {
        var result = JsonConvert.SerializeObject(new
        {
            StatusCode = statusCode,
            Message = message
        });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        return context.Response.WriteAsync(result);
    }
}
