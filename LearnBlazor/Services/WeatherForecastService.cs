using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LearnBlazor.Models;

namespace LearnBlazor.Services;

public class WeatherForecastService
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm",
        "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    public async Task<IReadOnlyList<WeatherForecast>> GetForecastAsync(CancellationToken cancellationToken = default)
    {
        await Task.Delay(500, cancellationToken);

        var startDate = DateOnly.FromDateTime(DateTime.Now);

        var forecasts = Enumerable
            .Range(1, 5)
            .Select(index => new WeatherForecast(
                startDate.AddDays(index),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]))
            .ToArray();

        return forecasts;
    }
}
