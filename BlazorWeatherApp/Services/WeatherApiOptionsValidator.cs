using System.Collections.Generic;
using BlazorWeatherApp.Models;
using Microsoft.Extensions.Options;

namespace BlazorWeatherApp.Services;

public class WeatherApiOptionsValidator : IValidateOptions<WeatherApiOptions>
{
    public ValidateOptionsResult Validate(string? name, WeatherApiOptions options)
    {
        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            failures.Add("WeatherApi:ApiKey must be provided.");
        }

        if (string.IsNullOrWhiteSpace(options.BaseUrl))
        {
            failures.Add("WeatherApi:BaseUrl must be provided.");
        }
        else if (!Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _))
        {
            failures.Add("WeatherApi:BaseUrl must be a valid absolute URI.");
        }

        return failures.Count > 0
            ? ValidateOptionsResult.Fail(failures)
            : ValidateOptionsResult.Success;
    }
}
