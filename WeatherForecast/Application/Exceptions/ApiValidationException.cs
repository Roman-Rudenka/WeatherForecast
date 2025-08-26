using WeatherForecast.Presentation.Common;

namespace WeatherForecast.Presentation.Exceptions;

public class ApiValidationException(string message) : ApiBaseException("Validation Error", 400, message);