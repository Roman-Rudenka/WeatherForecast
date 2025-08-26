using System.Net;

namespace WeatherForecast.Domain.Common;

public static class BaseExceptionHandler
{
    public static (int StatusCode, string Message) Handle(Exception ex)
    {
        (int, string) result;
        switch (ex)
        {
            case ArgumentException argEx :
                result = ((int)HttpStatusCode.BadRequest, argEx.Message);
                break;
            case KeyNotFoundException keyEx :
                result = ((int)HttpStatusCode.NotFound, keyEx.Message);
                break;
            default:
                result = ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
                break;
        }

        return result;
    }
}
