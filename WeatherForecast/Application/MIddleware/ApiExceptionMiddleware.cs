namespace WeatherForecast.Application.Middleware;

public class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }

        if (context.Response.StatusCode == 400)
        {
            await HandleBadRequestAsync(context);
        }
        else if (context.Response.StatusCode == 404)
        {
            await HandleNotFoundAsync(context);
        }
        else if (context.Response.StatusCode == 405)
        {
            await HandleMethodNotAllowedAsync(context);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        logger.LogError(ex, "Exception caught in middleware.");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var result = new 
        {
            error = "Internal Server Error",
            details = ex.Message
        };

        return context.Response.WriteAsJsonAsync(result);
    }

    private Task HandleBadRequestAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var result = new 
        {
            error = "Bad Request",
            details = "The request could not be understood by the server."
        };

        return context.Response.WriteAsJsonAsync(result);
    }

    private Task HandleNotFoundAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var result = new 
        {
            error = "Not Found",
            details = "The requested resource was not found."
        };

        return context.Response.WriteAsJsonAsync(result);
    }

    private Task HandleMethodNotAllowedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        var result = new 
        {
            error = "Method Not Allowed",
            details = "The method is not allowed for the requested resource."
        };

        return context.Response.WriteAsJsonAsync(result);
    }
}