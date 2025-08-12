using System.Text.Json;
using WeatherForecast.Domain.Models;

using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.Application.Services;

public class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "";

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Forecast> GetWeatherAsync(string? location, DateTime date)
    {
        var url = $"https://api.openweathermap.org/data/2.5/forecast?q={location}&appid={_apiKey}&units=metric";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync();
        var options =  new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var data = JsonSerializer.Deserialize<OpenWeatherForecastResponse>(json, options);
        
        var forecast = data?.List?.FirstOrDefault( x => DateTime.Parse(x.DtTxt).Date == date.Date);
        
        return new Forecast
        {
            Location = location,
            Date = date,
            TemperatureC = (int)(forecast?.Main.Temp ?? 0),
            Description = forecast?.Weather.FirstOrDefault()?.Description ?? "No data"
        };
    }
}