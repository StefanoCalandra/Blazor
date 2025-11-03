namespace BlazorWeatherApp.Services;

public class WeatherServiceException : Exception
{
    public WeatherServiceException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
