using System.Text.Json.Serialization;

namespace WeatherForecast.Domain.Models;

public class OpenWeatherForecastResponse
{
    [JsonPropertyName("list")]
    public List<ForecastItem> List { get; set; }
}

public class ForecastItem
{
    [JsonPropertyName("dt_txt")]
    public string DtTxt { get; set; }

    [JsonPropertyName("main")]
    public MainInfo Main { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherInfo> Weather { get; set; }
}

public class MainInfo
{
    [JsonPropertyName("temp")]
    public double Temp { get; set; }
}

public class WeatherInfo
{
    [JsonPropertyName("description")]
    public string Description { get; set; }
}