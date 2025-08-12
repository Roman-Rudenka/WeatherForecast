using WeatherForecast.Domain.Models;

namespace WeatherForecast.Application.Interfaces;

public interface IWeatherApiClient
{
    Task<Forecast> GetWeatherAsync(string? location, DateTime date);
}