using System.Text.Json;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Models;
using WeatherForecast.Application.Options;
using Microsoft.Extensions.Options;
using WeatherForecast.Presentation.Exceptions;

namespace WeatherForecast.Application.Services;

public class WeatherService(
    HttpClient client,
    IWeatherForecastRepository weatherForecastRepository,
    IOptions<WeatherApiOptions> options)
    : IWeatherService
{
    private readonly WeatherApiOptions _options = options.Value;


    public async Task<Forecast> GetTodayAsync(string address, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new ApiValidationException("Address is empty");
        }
        var date = DateOnly.FromDateTime(DateTime.Now);
        var existingForecast = await weatherForecastRepository.GetForecastByDateAndAddress(address, date, cancellationToken);
        
        if (existingForecast != null)
        {
            return existingForecast;
        }

        var newForecast = await GetWeatherAsync(address, date);
        await weatherForecastRepository.AddForecast(newForecast, cancellationToken);
        await weatherForecastRepository.SaveChanges(cancellationToken);
        
        return newForecast;   
    }

    public async Task<Forecast> GetByDateAsync(string address, DateOnly date, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new ApiValidationException("Address is empty");
        }
         var existingForecast = await weatherForecastRepository.GetForecastByDateAndAddress(address, date, cancellationToken);
         
         if (existingForecast != null)
         {
             return existingForecast;
         }
         
         var newForecast = await GetWeatherAsync(address, date);
         await weatherForecastRepository.AddForecast(newForecast,  cancellationToken);
         await weatherForecastRepository.SaveChanges(cancellationToken);
         
         return newForecast;
    }
    
    public async Task<ICollection<Forecast>> GetWeekAsync(string address, DateOnly date, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new ApiValidationException("Address is empty");
        }
        var existingWeekForecast =
            await weatherForecastRepository.GetWeekForecasts(address, date, date.AddDays(7), cancellationToken);
        if (existingWeekForecast.Count() == 7)
        {
            return existingWeekForecast;
        }
        
        var newWeekForecast  = await GetWeekWeatherAsync(address, date) as ICollection<Forecast>;
        if (newWeekForecast == null)
        {
            throw new NotFoundException("Cannot get week forecasts");
        }
        await weatherForecastRepository.AddForecasts(newWeekForecast, cancellationToken);
        await weatherForecastRepository.SaveChanges(cancellationToken);
        
        return  newWeekForecast;
    }

    private async Task<Forecast> GetWeatherAsync(string address, DateOnly date)
    {
        var formatedDate = date.ToString("yyyy-MM-dd");
        var baseUrl = GetBaseUrl();
        var apiKey = GetApiKey();
        var url = $"{baseUrl}/{address}/{formatedDate}/{formatedDate}?key={apiKey}";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var root = JsonSerializer.Deserialize<ForecastRoot>(json, options);
        if (root?.Days == null)
        {
            throw new Exception("Invalid forecast data.");
        }
        
        var day = root.Days.FirstOrDefault(d =>
            DateOnly.TryParse(d.Date, out var parsedDate) && parsedDate == date);
        
        if (day == null)
        {
            throw new Exception("No forecast data found for the specified date.");
        }

        return new Forecast
        {
            Address = address,
            Date = date,
            Lat = (float)root.Latitude,
            Lon = (float)root.Longitude,
            TemperatureC = ConvertFahrenheitToCelsius(day.Temperature),
            Description = day.Description
        };
    }
    
    private async Task<IEnumerable<Forecast?>> GetWeekWeatherAsync(string address, DateOnly date)
    {
        var dateStart = date.ToString("yyyy-MM-dd");
        var dateEnd = date.AddDays(7).ToString("yyyy-MM-dd");
        var baseUrl = GetBaseUrl();
        var apiKey = GetApiKey();
        var url = $"{baseUrl}/{address}/{dateStart}/{dateEnd}?key={apiKey}"; 
        var response = await client.GetAsync(url); 
        response.EnsureSuccessStatusCode();
        
        var json = await response.Content.ReadAsStringAsync(); 
        var options = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true
            
        };
        
        var root = JsonSerializer.Deserialize<ForecastRoot>(json, options); 
        if (root == null || root.Days == null) 
        { 
            throw new Exception("Invalid forecast data.");
            
        }
        
        var forecasts = root.Days.Select(d => 
            {
                if (!DateOnly.TryParse(d.Date, out var parsedDate))
                {
                    return null;
                }
                
                return new Forecast
                { 
                    Address = address, 
                    Date = parsedDate, 
                    Lat = (float)root.Latitude, 
                    Lon = (float)root.Longitude, 
                    TemperatureC = ConvertFahrenheitToCelsius(d.Temperature), 
                    Description = d.Description
                };
            })
            .Where(f => f != null).ToList();
        
        return forecasts;
    }

    
    private static double ConvertFahrenheitToCelsius(double tempF)
    {
        return Math.Round((tempF - 32) * 5 / 9, 1);
    }

    private string GetBaseUrl()
    {
        return _options.BaseUrl;
    }

    private string GetApiKey()
    {
        return _options.ApiKey;
    }

}