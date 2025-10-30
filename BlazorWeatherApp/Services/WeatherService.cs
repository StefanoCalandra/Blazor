using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using BlazorWeatherApp.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorWeatherApp.Services;

public class WeatherService
{
    private const string CityCacheKeyPrefix = "weather::city::";
    private const string CoordinateCacheKeyPrefix = "weather::coords::";

    private static readonly MemoryCacheEntryOptions CacheEntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
        SlidingExpiration = TimeSpan.FromMinutes(5)
    };

    private readonly HttpClient _httpClient;
    private readonly WeatherApiOptions _options;
    private readonly IMemoryCache _cache;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(
        HttpClient httpClient,
        IOptions<WeatherApiOptions> options,
        IMemoryCache cache,
        ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _cache = cache;
        _logger = logger;
    }

    public Task<WeatherResult> GetWeatherAsync(
        string city,
        bool forceRefresh = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new WeatherServiceException("Please provide a city before requesting the forecast.");
        }

        var normalizedCity = city.Trim();
        var cacheKey = CityCacheKeyPrefix + normalizedCity.ToUpperInvariant();

        return GetOrFetchAsync(
            cacheKey,
            ct => FetchAsync($"?key={_options.ApiKey}&q={Uri.EscapeDataString(normalizedCity)}", ct),
            forceRefresh,
            cancellationToken);
    }

    public Task<WeatherResult> GetWeatherForCoordinatesAsync(
        GeoCoordinates coordinates,
        bool forceRefresh = false,
        CancellationToken cancellationToken = default)
    {
        if (!IsValidLatitude(coordinates.Latitude) || !IsValidLongitude(coordinates.Longitude))
        {
            throw new WeatherServiceException("Coordinates must be within valid latitude/longitude ranges.");
        }

        var cacheKey = CoordinateCacheKeyPrefix + FormattableString.Invariant($"{coordinates.Latitude:F4}:{coordinates.Longitude:F4}");

        var query = Uri.EscapeDataString(FormattableString.Invariant(
            $"{coordinates.Latitude.ToString(CultureInfo.InvariantCulture)},{coordinates.Longitude.ToString(CultureInfo.InvariantCulture)}"));

        return GetOrFetchAsync(
            cacheKey,
            ct => FetchAsync($"?key={_options.ApiKey}&q={query}", ct),
            forceRefresh,
            cancellationToken);
    }

    public Task<WeatherResult> GetDefaultCityWeatherAsync(
        bool forceRefresh = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.DefaultCity))
        {
            throw new WeatherServiceException("No default city is configured. Provide a city or update WeatherApi:DefaultCity.");
        }

        return GetWeatherAsync(_options.DefaultCity, forceRefresh, cancellationToken);
    }

    private async Task<WeatherResult> GetOrFetchAsync(
        string cacheKey,
        Func<CancellationToken, Task<WeatherResponse>> factory,
        bool forceRefresh,
        CancellationToken cancellationToken)
    {
        if (!forceRefresh && _cache.TryGetValue(cacheKey, out CachedWeatherResponse? cached) && cached is not null)
        {
            return new WeatherResult(cached.Response, true, cached.RetrievedAt);
        }

        var response = await factory(cancellationToken).ConfigureAwait(false);
        var entry = new CachedWeatherResponse(response, DateTimeOffset.UtcNow);
        _cache.Set(cacheKey, entry, CacheEntryOptions);
        return new WeatherResult(entry.Response, false, entry.RetrievedAt);
    }

    private async Task<WeatherResponse> FetchAsync(string requestUri, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(requestUri, cancellationToken).ConfigureAwait(false);

            if (response is null)
            {
                throw new WeatherServiceException("The weather service returned an empty response. Please try again.");
            }

            return response;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning(ex, "Weather API request failed for {RequestUri}", requestUri);
            throw new WeatherServiceException("Unable to reach the weather service. Check your network connection and API key.", ex);
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(ex, "Unsupported content type returned for {RequestUri}", requestUri);
            throw new WeatherServiceException("The weather service returned an unsupported payload.", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Invalid JSON returned for {RequestUri}", requestUri);
            throw new WeatherServiceException("The weather service returned malformed data.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while fetching weather data for {RequestUri}", requestUri);
            throw new WeatherServiceException("An unexpected error occurred while retrieving the forecast.", ex);
        }
    }

    private static bool IsValidLatitude(double value) => value is >= -90 and <= 90;

    private static bool IsValidLongitude(double value) => value is >= -180 and <= 180;

    private sealed record CachedWeatherResponse(WeatherResponse Response, DateTimeOffset RetrievedAt);
}
