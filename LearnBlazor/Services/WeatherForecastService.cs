using System.Linq;
using System.Runtime.CompilerServices;
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
            .Select(index => CreateForecast(startDate.AddDays(index)))
            .ToArray();

        return forecasts;
    }

    public async IAsyncEnumerable<WeatherForecast> StreamForecastAsync(
        int updates = 5,
        TimeSpan? interval = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        interval ??= TimeSpan.FromSeconds(1);
        var startDate = DateOnly.FromDateTime(DateTime.Now);

        for (var index = 0; index < updates; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Delay(interval.Value, cancellationToken);

            yield return CreateForecast(startDate.AddDays(index));
        }
    }

    private static WeatherForecast CreateForecast(DateOnly date) => new(
        date,
        Random.Shared.Next(-20, 55),
        Summaries[Random.Shared.Next(Summaries.Length)]);
}
