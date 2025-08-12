using WeatherForecast.Domain.Models;

namespace WeatherForecast.Application.Interfaces;

public interface IWeatherService
{
    Task<Forecast> GetTodayAsync(string? location, HttpContext context);
    Task <IEnumerable<Forecast>> GetWeekAsync(string? location, HttpContext context);
    Task<IEnumerable<Forecast>> GetMonthAsync(string? location, HttpContext context);
    Task <Forecast> GetByDateAsync(string? location, HttpContext context, DateTime date);
}