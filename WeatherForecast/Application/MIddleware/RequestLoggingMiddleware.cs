namespace WeatherForecast.Application.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;

        if (request.Path.StartsWithSegments("/api"))
        {
            var method = request.Method;
            var path = request.Path;
            var port = context.Connection.LocalPort;

            await next(context);

            var statusCode = context.Response.StatusCode;

            logger.LogInformation("[{Time}] Method: {Method} Path: {Path} Port: {Port} StatusCode: {StatusCode}",
                                    DateTime.Now, method, path, port, statusCode);
        }
        else
        {
            await next(context);
        }
    }
}

