using WeatherForecast.Domain.Models;

namespace WeatherForecast.Application.Interfaces;

public interface IWeatherService
{ 
    public Task<Forecast> GetTodayAsync(string address, CancellationToken cancellationToken);
    public Task <Forecast> GetByDateAsync(string address, DateOnly date, CancellationToken cancellationToken); 
    public Task <IEnumerable<Forecast>> GetWeekAsync(string address, DateOnly date, CancellationToken cancellationToken);
}