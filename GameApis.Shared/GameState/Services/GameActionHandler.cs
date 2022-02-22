using GameApis.Shared.Dtos;
using GameApis.Shared.Players.Services;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

internal class GameActionHandler<TGameContext> : IGameActionHandler<TGameContext>
{
    private readonly IGameRepository<TGameContext> gameRepository;
    private readonly IInternalPlayerIdResolver playerIdResolver;
    private readonly IPlayerRepository playerRepository;
    private readonly ILogger<GameActionHandler<TGameContext>> logger;

    public GameActionHandler(
        IGameRepository<TGameContext> gameRepository,
        IInternalPlayerIdResolver playerIdResolver,
        IPlayerRepository playerRepository,
        ILogger<GameActionHandler<TGameContext>> logger)
    {
        this.gameRepository = gameRepository;
        this.playerIdResolver = playerIdResolver;
        this.playerRepository = playerRepository;
        this.logger = logger;
    }

    public async Task<OneOf<Success, ActionFailed>> HandleGameActionAsync(GameId gameId, IAction gameAction)
    {
        var result = await gameRepository.GetGameEngineAsync(gameId);
        if (result.TryPickT1(out _, out var gameEngine))
        {
            return new ActionFailed("The game was not found.");
        }

        var playerIdResult = await playerIdResolver.ResolveInternalPlayerIdAsync();
        if (playerIdResult.TryPickT1(out _, out var playerId))
        {
            return new ActionFailed("Not logged in");
        }
        var playerResult = await playerRepository.GetPlayerByInternalIdAsync(playerId);
        if (playerResult.TryPickT1(out _, out var player))
        {
            return new ActionFailed("Player not found");
        }

        try
        {
            return await gameEngine.HandleActionAsync(player.Id, gameAction);
        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Error during handling game action.");
            return new ActionFailed("Failed to process your action.");
        }
    }
}
