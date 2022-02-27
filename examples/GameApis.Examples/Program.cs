using Blazored.LocalStorage;
using GameApis.Examples;
using GameApis.Examples.Client;
using GameApis.Examples.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new GameApiClient("https://localhost:7223", new HttpClient(), sp.GetRequiredService<IPlayerIdAccessor>()));
builder.Services.AddScoped<IPlayerIdAccessor, PlayerIdAccessor>();
builder.Services.AddScoped<IGameRegistry, GameRegistry>();
builder.Services.AddAntDesign();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();