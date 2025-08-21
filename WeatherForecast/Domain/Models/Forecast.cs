namespace WeatherForecast.Domain.Models;

public class Forecast
{
    public string Id { get; set; } = Guid.CreateVersion7().ToString();
    public required string Address { get; set; }
    public float Lon { get; set; }
    public float Lat { get; set; }
    public DateOnly Date { get; set; }
    public double TemperatureC { get; set; }
    public string? Description { get; set; }
}

