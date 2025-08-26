using WeatherForecast.Presentation.Common;

namespace WeatherForecast.Presentation.Exceptions;

public class NotFoundException(string message) : ApiBaseException("not found", 404, message);