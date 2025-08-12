namespace WeatherForecast.Domain.Models;

public class Forecast
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string Description { get; set; }
}