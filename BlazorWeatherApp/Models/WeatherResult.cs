namespace BlazorWeatherApp.Models;

public sealed record WeatherResult(WeatherResponse Response, bool FromCache, DateTimeOffset RetrievedAt);
