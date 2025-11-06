namespace BlazorWeatherApp.Models;

public readonly record struct GeoCoordinates(double Latitude, double Longitude, double? AccuracyMeters = null)
{
    public override string ToString() => AccuracyMeters is null
        ? $"{Latitude:F4}, {Longitude:F4}"
        : $"{Latitude:F4}, {Longitude:F4} (±{AccuracyMeters.Value:F0} m)";
}
