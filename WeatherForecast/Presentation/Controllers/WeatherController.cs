using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.Presentation.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _service;
    private readonly ILocationResolver _locationResolver;

    public WeatherController(IWeatherService service, ILocationResolver locationResolver)
    {
        _service = service;
        _locationResolver = locationResolver;
    }

    [HttpPost("today")]
    public async Task<IActionResult> GetToday([FromBody] string? location) =>
        Ok(await _service.GetTodayAsync(location, HttpContext));

    [HttpPost("date")]
    public async Task<IActionResult> GetByDate([FromBody] string location, DateTime date) =>
        Ok(await _service.GetByDateAsync(location, HttpContext, date)); 

    [HttpPost("week")]
    public async Task<IActionResult> GetWeek([FromBody] string? location) =>
        Ok(await _service.GetWeekAsync(location, HttpContext));

    [HttpPost("month")]
    public async Task<IActionResult> GetMonth([FromBody] string? location) =>
        Ok(await _service.GetMonthAsync(location, HttpContext));

    [HttpPost("location")]
    public async Task<IActionResult> GetLocation([FromBody] string? location)
    {
        var resolved = await _locationResolver.ResolveLocationAsync(location, HttpContext);
        return Ok(new { city = resolved });
    }
}