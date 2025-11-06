namespace BlazorWeatherApp.Models;

public class WeatherApiOptions
{
    public required string BaseUrl { get; set; }

    public required string ApiKey { get; set; }

    /// <summary>
    /// Città predefinita facoltativa usata quando l'utente non ha specificato una ricerca esplicita.
    /// </summary>
    public string? DefaultCity { get; set; }
}