using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.Presentation.Controllers;



[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpPost("today")]
    public async Task<IActionResult> GetToday([FromBody] string address)
    {
        return Ok(await _weatherService.GetTodayAsync(address, CancellationToken.None));
    }

    [HttpPost("day")]
    public async Task<IActionResult> GetDay([FromBody] string address, DateOnly date)
    {
        return Ok(await _weatherService.GetByDateAsync(address, date,  CancellationToken.None));
    }

    [HttpPost("week")]
    public async Task<IActionResult> GetWeek([FromBody] string address, DateOnly date)
    {
        return Ok(await _weatherService.GetWeekAsync(address, date,  CancellationToken.None));
    }
}
