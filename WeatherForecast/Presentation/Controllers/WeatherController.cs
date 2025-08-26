using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Presentation.Exceptions;

namespace WeatherForecast.Presentation.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    [HttpPost("today")]
    public async Task<IActionResult> GetToday([FromBody] string address, CancellationToken cancellationToken)
    {
        string getAdress = address;
        if (string.IsNullOrEmpty(getAdress))
        {
            throw new ApiValidationException("Address is empty");
        }
        var weatherToday = await weatherService.GetTodayAsync(getAdress, cancellationToken); 
        
        return Ok(weatherToday);
    }

    [HttpPost("day")]
    public async Task<IActionResult> GetDay([FromBody] string address, DateOnly date,  CancellationToken cancellationToken)
    {
        string getAdress = address;
        if (string.IsNullOrEmpty(getAdress))
        {
            throw new ApiValidationException("Address is empty");
        }
        var weatherOfTheDay = await weatherService.GetByDateAsync(getAdress, date, cancellationToken);
        
        return Ok(weatherOfTheDay);
    }

    [HttpPost("week")]
    public async Task<IActionResult> GetWeek([FromBody] string address, DateOnly date, CancellationToken cancellationToken)
    {
        string getAdress = address;
        if (string.IsNullOrEmpty(getAdress))
        {
            throw new ApiValidationException("Address is empty");
        }
        var weatherOfTheWeek = await weatherService.GetWeekAsync(getAdress, date,  cancellationToken);
        
        return Ok(weatherOfTheWeek);
    }
}
