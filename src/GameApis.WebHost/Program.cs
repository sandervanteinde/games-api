using GameApis.MongoDb;
using GameApis.Shared;
using GameApis.Shared.GameState.Services;
using GameApis.Shared.Players;
using GameApis.Shared.Players.Services;
using GameApis.TicTacToe.GameState;
using GameApis.WebHost;
using GameApis.WebHost.Models;
using GameApis.WebHost.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});
builder.Services.AddSwaggerGen(opts =>
{
    var playerIdSecurityScheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Player-Id"
    };
    opts.AddSecurityDefinition("PlayerId", playerIdSecurityScheme);
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "PlayerId"
            }
        }] = new List<string>()
    });
});
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
app.UseCors(opts => opts.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

// app.UseTicTacToeEndpoints();
var gameRegistry = app.Services.GetRequiredService<IGameRegistry>();
foreach (var entry in gameRegistry.EnumerateGameRegistryEntries())
{
    app.MapPost($"/api/{entry.Identifier}", EndpointHandlers.HandleCreateGame(entry.GameContextType, entry.InitialState));
    app.MapGet($"/api/{entry.Identifier}/{{gameId:guid}}", EndpointHandlers.HandleGetGame(entry.GameContextType))
        .Produces(200, entry.GameContextType)
        .Produces(404);

    foreach (var action in entry.Actions)
    {
        app.MapPost($"/api/{entry.Identifier}/{{gameId:guid}}/actions/{action.Name}", EndpointHandlers.HandleActionEndpoint(entry.GameContextType, action))
            .Produces(201);
    }
}

app.MapPost("/api/player", async (CreateNewPlayer body, IPlayerRepository playerRepository) =>
{
    var playerId = PlayerId.New();
    var player = new Player(playerId, body.PlayerName);
    await playerRepository.StorePlayerAsyc(player);
    return player.Id;
});
app.Run();