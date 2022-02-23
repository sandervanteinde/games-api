using GameApis.Examples.TicTacToe;
using GameApis.Examples.TicTacToe.Client;
using GameApis.Examples.TicTacToe.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new TicTacToeClient("https://localhost:7223", new HttpClient(), sp.GetRequiredService<IPlayerIdAccessor>()));
builder.Services.AddSingleton<IPlayerIdAccessor, PlayerIdAccessor>();
builder.Services.AddAntDesign();

await builder.Build().RunAsync();
