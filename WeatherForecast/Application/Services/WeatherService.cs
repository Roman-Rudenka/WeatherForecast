using System.Text.Json;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Models;

namespace WeatherForecast.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherApiClient _weatherApiClient;
    private readonly HttpClient _httpClient;

    public WeatherService(IWeatherApiClient weatherApiClient,  HttpClient httpClient)
    {
        _weatherApiClient = weatherApiClient;
        _httpClient = httpClient;
    }
    
    private async Task<string> ResolveLocationAsync(string? location, HttpContext context)
    {
        if (!string.IsNullOrWhiteSpace(location))
            return location;

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "auto";
        var response = await _httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");

        using var doc = JsonDocument.Parse(response);
        var root = doc.RootElement;

        if (root.TryGetProperty("city", out var cityElement))
            return cityElement.GetString() ?? "Unknown";

        return "Unknown";
    }
    
    public async Task<Forecast> GetTodayAsync(string? location, HttpContext context)
    {
        var resolvedLocation = await ResolveLocationAsync(location, context);
        return await _weatherApiClient.GetWeatherAsync(resolvedLocation, DateTime.UtcNow.Date);
    }

    public async Task<Forecast> GetByDateAsync(string? location, HttpContext context, DateTime date)
    {
        var resolvedLocation = await ResolveLocationAsync(location, context);
        return await _weatherApiClient.GetWeatherAsync(resolvedLocation, date.Date);
    }

    public async Task<IEnumerable<Forecast>> GetWeekAsync(string? location, HttpContext context)
    {
        var resolvedLocation = await ResolveLocationAsync(location, context);
        var today = DateTime.UtcNow.Date;

        var tasks = Enumerable.Range(0, 7)
            .Select(offset => _weatherApiClient.GetWeatherAsync(resolvedLocation, today.AddDays(offset)));

        return await Task.WhenAll(tasks);
    }

    public async Task<IEnumerable<Forecast>> GetMonthAsync(string? location, HttpContext context)
    {
        var resolvedLocation = await ResolveLocationAsync(location, context);
        var today = DateTime.UtcNow.Date;

        var tasks = Enumerable.Range(0, 30)
            .Select(offset => _weatherApiClient.GetWeatherAsync(resolvedLocation, today.AddDays(offset)));

        return await Task.WhenAll(tasks);
    }
}