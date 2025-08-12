using System.Text.Json;
using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.Application.Services;

public class LocationResolver :  ILocationResolver
{
    private readonly HttpClient _httpClient;

    public LocationResolver(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> ResolveLocationAsync(string? location, HttpContext context)
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
}