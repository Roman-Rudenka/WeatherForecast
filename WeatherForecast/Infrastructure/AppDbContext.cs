using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WeatherForecast.Domain.Models;

namespace WeatherForecast.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Forecast> Forecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AppDbContext)) ?? Assembly.GetExecutingAssembly());
    }
}