using WeatherForecast.Domain.Models;

namespace WeatherForecast.Application.Interfaces;

public interface IWeatherForecastRepository
 {
     public Task AddForecast(Forecast forecast, CancellationToken cancellationToken);
     public Task<Forecast> GetForecastByDateAndAddress(string address, DateOnly date, CancellationToken cancellationToken);
     public Task<IEnumerable<Forecast>> GetWeekForecasts(string address, DateOnly firstDay, DateOnly lastDay, CancellationToken cancellationToken);
     public Task AddForecasts(IEnumerable<Forecast> forecasts, CancellationToken cancellationToken);
}