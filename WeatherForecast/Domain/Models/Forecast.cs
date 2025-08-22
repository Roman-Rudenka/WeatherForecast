namespace WeatherForecast.Domain.Models;

public class Forecast
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public required string Address { get; init; }
    public float Lon { get; init; }
    public float Lat { get; init; }
    public DateOnly Date { get; init; }
    public double TemperatureC { get; init; }
    public string? Description { get; init; }
}

