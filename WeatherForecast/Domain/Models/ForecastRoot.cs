using System.Text.Json.Serialization;

namespace WeatherForecast.Domain.Models;

public class ForecastRoot
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<DayForecast> Days { get; set; }

}

public class DayForecast
{
    [JsonPropertyName("datetime")]
    public string Date { get; set; }
    [JsonPropertyName("temp")]
    public double Temperature { get; set; }
    public string Description { get; set; } 
}