using GameApis.SecretHitler.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace GameApis.SecretHitler.Api;

public static class EndpointRouteBuilderExtensions
{
    private const string GAME_ENDPOINT = "api/secret-hitler/game";
    private const string GAME_ID_ENDPOINT = $"{GAME_ENDPOINT}/{{gameId}}";
    private const string PLAYER_ENDPOINT = $"{GAME_ID_ENDPOINT}/players";
    private const string PLAYER_ID_ENDPOINT = $"{PLAYER_ENDPOINT}/{{playerId}}";
    public static IEndpointRouteBuilder MapSecretHitlerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(GAME_ENDPOINT, async (ISecretHitlerGameService service) =>
        {
            var createdGame = await service.CreateNewAsync();
            return createdGame;
        }).WithDisplayName("Create game").WithTags("Secret Hitler");
        endpoints.MapGet(GAME_ID_ENDPOINT, async (Guid gameId, ISecretHitlerGameService service) =>
        {
            var game = await service.GetGameAsync(gameId);
            return game;
        }).WithDisplayName("Get Game by Id").WithTags("Secret Hitler");
        endpoints.MapPost(GAME_ID_ENDPOINT, async (Guid gameId, ISecretHitlerGameService service) =>
        {
            var game = await service.UpdateSecretHitlerGameAsync(gameId, game => game.StartGame());
            return game;
        }).WithDisplayName("Start game").WithTags("Secret Hitler");
        endpoints.MapPost(PLAYER_ENDPOINT, async (Guid gameId, [FromBody] AddPlayer body, ISecretHitlerGameService service) =>
        {
            var game = await service.UpdateSecretHitlerGameAsync(gameId, game => game.AddPlayer(body.PlayerName));
            return game;
        }).WithDisplayName("Add player").WithTags("Secret Hitler");
        endpoints.MapDelete(PLAYER_ID_ENDPOINT, async (Guid gameId, Guid playerId, ISecretHitlerGameService service) =>
        {
            var game = await service.UpdateSecretHitlerGameAsync(gameId, game => game.PlayerLeaves(playerId));
            return game;
        }).WithDisplayName("Player Leaves").WithTags("Secret Hitler");
        return endpoints;
    }

    public record AddPlayer(string PlayerName);
}