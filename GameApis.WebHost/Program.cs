using GameApis.MongoDb;
using GameApis.Shared;
using GameApis.Shared.GameState.Services;
using GameApis.Shared.Players.Services;
using GameApis.TicTacToe.GameState;
using GameApis.WebHost;
using GameApis.WebHost.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddGameApisMongoDbPersistence(builder.Configuration.GetConnectionString("MongoDb"));
builder.Services.AddGameApisServices(builder => builder.RegisterGameContext<TicTacToeContext>());
builder.Services.AddScoped<IInternalPlayerIdResolver, InternalPlayerIdResolver>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

// app.UseTicTacToeEndpoints();
var gameRegistry = app.Services.GetRequiredService<IGameRegistry>();
foreach (var entry in gameRegistry.EnumerateGameRegistryEntries())
{
    app.MapPost($"/api/{entry.Identifier}", EndpointHandlers.HandleCreateGame(entry.GameContextType, entry.InitialState));
    app.MapGet($"/api/{entry.Identifier}/{{gameId:guid}}", EndpointHandlers.HandleGetGame(entry.GameContextType));

    foreach (var action in entry.Actions)
    {
        app.MapPost($"/api{entry.Identifier}/{{gameId:guid}}/actions/{action.Name}", EndpointHandlers.HandleActionEndpoint(entry.GameContextType, action));
    }
}

app.Run();