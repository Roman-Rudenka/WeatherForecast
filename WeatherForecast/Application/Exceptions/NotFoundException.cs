using WeatherForecast.Application.Common;

namespace WeatherForecast.Application.Exceptions;

public class NotFoundException(string message) : ApiBaseException("not found", 404, message);