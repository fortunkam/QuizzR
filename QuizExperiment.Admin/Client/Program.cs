using BlazorApplicationInsights;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Plk.Blazor.DragDrop;
using QuizExperiment.Admin.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazorDragDrop();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services
      .AddBlazorise(options =>
      {
          options.Immediate = true;
      })
      .AddBootstrapProviders()
      .AddFontAwesomeIcons();

builder.Services.AddSingleton(new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

builder.Services.AddBlazorApplicationInsights(async applicationInsights =>
{
    var key = builder.Configuration["AppInsightsInstrumentationKey"];
    await applicationInsights.SetInstrumentationKey(key);
    var telemetryItem = new TelemetryItem()
    {
        Tags = new Dictionary<string, object>()
        {
            { "ai.cloud.role", "SPA" },
            { "ai.cloud.roleInstance", "Blazor Wasm" },
        }
    };

    await applicationInsights.AddTelemetryInitializer(telemetryItem);
    await applicationInsights.TrackPageView();
});

await builder.Build().RunAsync();
