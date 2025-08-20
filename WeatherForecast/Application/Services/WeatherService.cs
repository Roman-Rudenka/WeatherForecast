using System.Text.Json;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Models;

namespace WeatherForecast.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _client;
    private readonly IWeatherForecastRepository _weatherForecastRepository;
    private readonly IConfiguration _configuration;
    public WeatherService(HttpClient client,  IWeatherForecastRepository weatherForecastRepository,  IConfiguration configuration)
    {
        _client = client;
        _weatherForecastRepository = weatherForecastRepository;
        _configuration = configuration;
    }

    public async Task<Forecast> GetTodayAsync(string address, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(address))
        {
            return null;
        }
        var date = DateOnly.FromDateTime(DateTime.Now);
        var existingForecast = await _weatherForecastRepository.GetForecastByDateAndAddress(address, date, CancellationToken.None);
        if (existingForecast != null)
        {
            return existingForecast;
        }

        var formatedDateStart = date.ToString("yyyy-MM-dd");
        var formatedDateEnd = formatedDateStart;
        var newForecast = await GetWeatherAsync(address, date);
        await _weatherForecastRepository.AddForecast(newForecast, CancellationToken.None);
        return newForecast;   
    }

    public async Task<Forecast> GetByDateAsync(string address, DateOnly date, CancellationToken cancellationToken)
    {
         if (string.IsNullOrEmpty(address))
         {
             return null;
         }
         var existingForecast = await _weatherForecastRepository.GetForecastByDateAndAddress(address, date, CancellationToken.None);
         if (existingForecast != null)
         {
             return existingForecast;
         }
        
        var formatedDateStart = date.ToString("yyyy-MM-dd");
         var formatedDateEnd = formatedDateStart;
         var newForecast = await GetWeatherAsync(address, date);
         await _weatherForecastRepository.AddForecast(newForecast,  CancellationToken.None);
         return newForecast;
    }
    
    public async Task<IEnumerable<Forecast>> GetWeekAsync(string address, DateOnly date, CancellationToken cancellationToken)
    {
        var formatedDateStart = date.ToString("yyyy-MM-dd");
        var formatedDateEnd = date.AddDays(7).ToString("yyyy-MM-dd");
        if (string.IsNullOrEmpty(address))
        {
            return null;
        }
        var existingWeekForecast = await _weatherForecastRepository.GetWeekForecasts(address, date, date.AddDays(7), CancellationToken.None);
        if (existingWeekForecast != null && existingWeekForecast.Any() && existingWeekForecast.Count() == 7)
        {
            return existingWeekForecast;
        }
        
        var newWeekForecast = await GetWeekWeatherAsync(address, date);
        await _weatherForecastRepository.AddForecasts(newWeekForecast, CancellationToken.None);
        return  newWeekForecast;
    }

    public async Task<Forecast> GetWeatherAsync(string address, DateOnly date)
    {
        var formatedDate = date.ToString("yyyy-MM-dd");
        var baseUrl = _configuration["WeatherApi:BaseUrl"];
        var apiKey = _configuration["WeatherApi:apiKey"];
        var url = $"{baseUrl}/{address}/{formatedDate}/{formatedDate}?key={apiKey}";
        Console.WriteLine(url);
        var response = await _client.GetAsync(url);
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
    
    public async Task<IEnumerable<Forecast>> GetWeekWeatherAsync(string address, DateOnly date)
    {
        var dateStart = date.ToString("yyyy-MM-dd");
        var dateEnd = date.AddDays(7).ToString("yyyy-MM-dd");
        var baseUrl = _configuration["WeatherApi:BaseUrl"];
        var apiKey = _configuration["WeatherApi:apiKey"];
        var url = $"{baseUrl}/{address}/{dateStart}/{dateEnd}?key={apiKey}"; 
        var response = await _client.GetAsync(url); 
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

    
    private double ConvertFahrenheitToCelsius(double tempF)
    {
        return Math.Round((tempF - 32) * 5 / 9, 1);
    }
}