using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain.Models;
using WeatherForecast.Application.Interfaces;
using System.Linq;

namespace WeatherForecast.Infrastructure.Repositories;

public class ForecastRepository(AppDbContext context) : IWeatherForecastRepository
{
    public async Task AddForecast(Forecast forecast,  CancellationToken cancellationToken)
    {
        await context.Set<Forecast>().AddAsync(forecast, cancellationToken);
    }

    public async Task<Forecast?> GetForecastByDateAndAddress(string address, DateOnly date,  CancellationToken cancellationToken)
    {
        return await context.Forecasts.FirstOrDefaultAsync(f => f.Date == date && f.Address == address, cancellationToken: cancellationToken);
    }
    
    public async Task<ICollection<Forecast>> GetWeekForecasts(string address, DateOnly firstDay, DateOnly lastDay, CancellationToken cancellationToken)
    {
         return await context.Forecasts.Where(f => f.Date >= firstDay && f.Date <= lastDay && f.Address == address).ToListAsync(cancellationToken);
    }

    
    public async Task AddForecasts(ICollection<Forecast> forecasts,  CancellationToken cancellationToken)
    {
        await context.Forecasts.AddRangeAsync(forecasts, cancellationToken);
    }

    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}