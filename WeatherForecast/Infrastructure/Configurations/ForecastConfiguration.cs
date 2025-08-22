using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WeatherForecast.Domain.Models;

namespace WeatherForecast.Infrastructure.Configurations;

public class ForecastConfiguration : IEntityTypeConfiguration<Forecast>
{
    public void Configure(EntityTypeBuilder<Forecast> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Address).HasMaxLength(100);
        builder.Property(f => f.Lon).IsRequired();
        builder.Property(f => f.Lat).IsRequired();
        builder.Property(f => f.Date).IsRequired();
        builder.Property(f => f.TemperatureC).IsRequired();
        builder.Property(f => f.Description).IsRequired().HasMaxLength(500);
        builder.ToTable("Forecasts");
    }
}