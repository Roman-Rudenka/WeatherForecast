namespace WeatherForecast.Application.Options;

public class WeatherApiOptions
{
    public required string BaseUrl { get;  init; }
    public required string ApiKey { get;  init; }
}