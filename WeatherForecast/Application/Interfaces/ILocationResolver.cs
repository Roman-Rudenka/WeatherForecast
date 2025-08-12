namespace WeatherForecast.Application.Interfaces;

public interface ILocationResolver
{ 
    Task<string> ResolveLocationAsync(string? location, HttpContext context);
}