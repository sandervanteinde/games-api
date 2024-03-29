using GameApis.Games.TicTacToe;
using GameApis.MongoDb;
using GameApis.Shared;
using GameApis.Shared.Dtos;
using GameApis.Shared.GameState.Services;
using GameApis.Shared.Players;
using GameApis.Shared.Players.Services;
using GameApis.WebHost;
using GameApis.WebHost.Models;
using GameApis.WebHost.Services;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JsonOptions>(
    options =>
    {
        options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }
);
builder.Services.AddControllers()
    .AddJsonOptions(
        options =>
        {
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        }
    );
builder.Services.AddSwaggerGen(
    opts =>
    {
        var playerIdSecurityScheme = new OpenApiSecurityScheme { In = ParameterLocation.Header, Type = SecuritySchemeType.ApiKey, Name = "X-Player-Id" };
        opts.AddSecurityDefinition("PlayerId", playerIdSecurityScheme);
        opts.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "PlayerId" } }, new List<string>()
                }
            }
        );
    }
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddGameApisMongoDbPersistence(
    builder.Configuration.GetConnectionString("MongoDb") ?? throw new InvalidOperationException("Missing MongoDb connection string")
);
builder.Services.AddGameApisServices(gameBuilder => gameBuilder.RegisterGameContext<TicTacToeContext>());
builder.Services.AddScoped<IInternalPlayerIdResolver, InternalPlayerIdResolver>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c =>
        {
            c.RoutePrefix = string.Empty;
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameApis.WebHost v1");
        }
    );
}

app.UseCors(
    opts => opts.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
);
app.UseHttpsRedirection();

var gameRegistry = app.Services.GetRequiredService<IGameRegistry>();
var getGameContextType = typeof(GetGameResult<>);
app.MapGet(
        "/api/games", (IGameRegistry registry) =>
        {
            return Results.Ok(
                registry.EnumerateGameRegistryEntries()
                    .Select(entry => entry.Identifier)
            );
        }
    )
    .Produces(200, typeof(IEnumerable<string>))
    .WithName("GetAvailableGames")
    .WithTags("AvailableGames");

foreach (var entry in gameRegistry.EnumerateGameRegistryEntries())
{
    app.MapPost(
            $"/api/{entry.Identifier}",
            EndpointHandlers.HandleCreateGame(entry.GameContextType)
        )
        .Produces(200, typeof(Guid))
        .WithName($"Create{entry.Identifier}Game")
        .WithTags(entry.Identifier);

    app.MapGet($"/api/{entry.Identifier}/{{gameId:guid}}", EndpointHandlers.HandleGetGame(entry.GameContextType))
        .Produces(200, getGameContextType.MakeGenericType(entry.GameContextType))
        .Produces(404)
        .WithName($"Get{entry.Identifier}GameById")
        .WithTags(entry.Identifier);

    foreach (var action in entry.Actions)
    {
        app.MapPost(
                $"/api/{entry.Identifier}/{{gameId:guid}}/actions/{action.Name}",
                EndpointHandlers.HandleActionEndpoint(entry.GameContextType, action)
            )
            .Produces(201)
            .Produces(400, typeof(ActionFailed))
            .WithName($"Perform{action.Name}On{entry.Identifier}Game")
            .WithTags(entry.Identifier);
    }
}

app.MapPost(
        "/api/player", async (CreateNewPlayer body, IPlayerRepository playerRepository) =>
        {
            var playerId = PlayerId.New();
            var player = new Player(playerId, body.PlayerName);
            await playerRepository.StorePlayerAsyc(player);
            return player.Id;
        }
    )
    .WithName("CreatePlayer")
    .WithTags("Players");

app.MapGet(
        "/api/player/me", async (IInternalPlayerIdResolver resolver, IPlayerRepository playerRepo) =>
        {
            var internalPlayerId = await resolver.ResolveInternalPlayerIdAsync();
            return await internalPlayerId.Match<Task<IResult>>(
                async playerId =>
                {
                    var playerOrNotFound = await playerRepo.GetPlayerByInternalIdAsync(playerId);
                    return playerOrNotFound.Match(
                        player => Results.Ok(new PlayerDto(player.Name, player.Id.ExternalId.Value)),
                        _ => Results.NotFound()
                    );
                },
                _ => Task.FromResult(Results.Unauthorized())
            );
        }
    )
    .Produces(200, typeof(PlayerDto))
    .Produces(401)
    .Produces(404)
    .WithName("GetMe")
    .WithTags("Players");

app.MapPost(
        "/api/player/exists", async (QueryPlayerInternalId body, IPlayerRepository playerRepository) =>
        {
            var internalPlayerId = new InternalPlayerId(body.InternalPlayerId);
            var player = await playerRepository.GetPlayerByInternalIdAsync(internalPlayerId);
            return player.Match(
                playerResult => Results.Ok(new PlayerDto(playerResult.Name, playerResult.Id.ExternalId.Value)),
                _ => Results.NotFound()
            );
        }
    )
    .Produces(404)
    .Produces(200, typeof(PlayerDto))
    .WithName("GetPlayerByInternalId")
    .WithTags("Players");

app.MapGet(
        "/api/player/{playerId:guid}", async (Guid playerId, IPlayerRepository playerRepository) =>
        {
            var externalPlayerId = new ExternalPlayerId(playerId);
            var player = await playerRepository.GetPlayerByExternalIdAsync(externalPlayerId);

            return player.Match(
                playerResult => Results.Ok(new PlayerDto(playerResult.Name, playerResult.Id.ExternalId.Value)),
                _ => Results.NotFound()
            );
        }
    )
    .Produces(404)
    .Produces(200, typeof(PlayerDto))
    .WithName("GetPlayerByExternalId")
    .WithTags("Players");

app.Run();