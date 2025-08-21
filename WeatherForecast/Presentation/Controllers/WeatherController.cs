using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Models;

namespace WeatherForecast.Presentation.Controllers;



[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherService weatherService) : ControllerBase
{
    [HttpPost("today")]
    public async Task<IActionResult> GetToday([FromBody] string address)
    {
        var weatherToday = await weatherService.GetTodayAsync(address, CancellationToken.None); 
        return Ok(weatherToday);
    }

    [HttpPost("day")]
    public async Task<IActionResult> GetDay([FromBody] string address, DateOnly date)
    {
        var weatherOfTheDay = await weatherService.GetByDateAsync(address, date, CancellationToken.None);
        return Ok(weatherOfTheDay);
    }

    [HttpPost("week")]
    public async Task<IActionResult> GetWeek([FromBody] string address, DateOnly date)
    {
        var weatherOfTheWeek = await weatherService.GetWeekAsync(address, date,  CancellationToken.None);
        return Ok(weatherOfTheWeek);
    }
}
