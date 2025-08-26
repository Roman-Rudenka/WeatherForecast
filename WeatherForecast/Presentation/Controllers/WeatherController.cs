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
        var weatherToday = await weatherService.GetTodayAsync(address, cancellationToken); 
        
        return Ok(weatherToday);
    }

    [HttpPost("day")]
    public async Task<IActionResult> GetDay([FromBody] string address, DateOnly date,  CancellationToken cancellationToken)
    {
        var weatherOfTheDay = await weatherService.GetByDateAsync(address, date, cancellationToken);
        
        return Ok(weatherOfTheDay);
    }

    [HttpPost("week")]
    public async Task<IActionResult> GetWeek([FromBody] string address, DateOnly date, CancellationToken cancellationToken)
    {

        var weatherOfTheWeek = await weatherService.GetWeekAsync(address, date,  cancellationToken);
        
        return Ok(weatherOfTheWeek);
    }
}
