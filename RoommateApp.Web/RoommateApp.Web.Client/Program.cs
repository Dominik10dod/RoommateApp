using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using RoommateApp.Web.Client;
using RoommateApp.Web.Client.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddRadzenComponents();
builder.Services.AddScoped<UserSessionService>();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddScoped<ApartmentClient>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new ApartmentClient(httpClient)
    {
        BaseUrl = builder.HostEnvironment.BaseAddress.TrimEnd('/'),
        ReadResponseAsString = true
    };
});

builder.Services.AddScoped<ApartmentStateService>();

// UsersClient regisztrációja
builder.Services.AddScoped<UsersClient>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new UsersClient(httpClient)
    {
        BaseUrl = builder.HostEnvironment.BaseAddress.TrimEnd('/'),
        ReadResponseAsString = true
    };
});

// ÚJ: ChoresClient regisztrációja
builder.Services.AddScoped<ChoresClient>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new ChoresClient(httpClient)
    {
        BaseUrl = builder.HostEnvironment.BaseAddress.TrimEnd('/'),
        ReadResponseAsString = true
    };
});

await builder.Build().RunAsync();