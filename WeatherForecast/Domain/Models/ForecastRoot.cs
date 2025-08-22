using System.Text.Json.Serialization;

namespace WeatherForecast.Domain.Models;

public class ForecastRoot
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public required List<DayForecast> Days { get; init; }

}

public class DayForecast(string date, double temperature, string? description)
{
    [JsonPropertyName("datetime")]
    public string Date { get; } = date;

    [JsonPropertyName("temp")]
    public double Temperature { get; } = temperature;

    public string? Description { get; } = description;
}