using WeatherForecast.Application.Common;

namespace WeatherForecast.Application.Exceptions;

public class ApiValidationException(string message) : ApiBaseException("Validation Error", 400, message);