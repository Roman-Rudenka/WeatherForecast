namespace WeatherForecast.Domain.Models;

public class LogsModel
{
    public string Id { get; set; } =  Guid.NewGuid().ToString();
    public DateTime Date { get; set; } =  DateTime.Now;
    public string Message { get; set; }
}