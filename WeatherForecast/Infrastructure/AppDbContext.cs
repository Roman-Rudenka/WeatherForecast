using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain.Models;
using WeatherForecast.Infrastructure.Configurations;

namespace WeatherForecast.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Forecast> Forecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ForecastConfiguration());
    }
}