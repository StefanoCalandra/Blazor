using BlazorWeatherApp.Models;
using BlazorWeatherApp.Components;
using BlazorWeatherApp.Services;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Aggiunge i servizi al contenitore.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddOptions<WeatherApiOptions>()
    .Bind(builder.Configuration.GetSection("WeatherApi"))
    .ValidateOnStart();

builder.Services.AddSingleton<IValidateOptions<WeatherApiOptions>, WeatherApiOptionsValidator>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<WeatherService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<WeatherApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
});
var app = builder.Build();

// Configura la pipeline delle richieste HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // Il valore HSTS predefinito è 30 giorni. Per gli scenari di produzione puoi modificarlo: vedi https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
