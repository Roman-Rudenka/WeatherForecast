namespace WeatherForecast.Application.Common;

public class ApiBaseException : Exception
{
    public string Status { get; set; }
 
    public int StatusCode { get; set; }
 
    public ApiBaseException(string status, int statusCode, string message) : base(message)
    {
        Status = status;
        StatusCode = statusCode;
    }  
}