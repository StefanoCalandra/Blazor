namespace BlazorWeatherApp.Models;

public class WeatherApiOptions
{
    public required string BaseUrl { get; set; }

    public required string ApiKey { get; set; }

    /// <summary>
    /// Optional default city used when the user has not provided an explicit search.
    /// </summary>
    public string? DefaultCity { get; set; }
}