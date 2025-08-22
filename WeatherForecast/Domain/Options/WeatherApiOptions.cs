namespace WeatherForecast.Domain.Options;

public class WeatherApiOptions
{
    public required string BaseUrl { get;  init; }
    public required string ApiKey { get;  init; }
}