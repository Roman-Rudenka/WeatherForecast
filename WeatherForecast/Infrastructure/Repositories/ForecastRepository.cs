using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain.Models;
using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.Infrastructure.Repositories;

public class ForecastRepository : IWeatherForecastRepository
{
    private readonly AppDbContext _context;

    public ForecastRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddForecast(Forecast forecast,  CancellationToken cancellationToken)
    {
        await _context.Forecasts.AddAsync(forecast);
        await _context.SaveChangesAsync();
    }

    public async Task<Forecast> GetForecastByDateAndAddress(string address, DateOnly date,  CancellationToken cancellationToken)
    {
        return await _context.Forecasts.FirstOrDefaultAsync(f => f.Date == date && f.Address == address);
    }

    public async Task<IEnumerable<Forecast>> GetWeekForecasts(string address, DateOnly firstDay, DateOnly lastDay, CancellationToken cancellationToken)
    {
        return await _context.Forecasts.Where(f => f.Date >= firstDay && f.Date <= lastDay && f.Address == address).ToListAsync();
    }
    
    public async Task AddForecasts(IEnumerable<Forecast> forecasts,  CancellationToken cancellationToken)
    {
        await _context.Forecasts.AddRangeAsync(forecasts);
        await _context.SaveChangesAsync();
    }

}