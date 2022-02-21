using GameApis.Shared.Dtos;
using GameApis.Shared.GameState;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Services;

internal class GameActionHandler<TGameContext> : IGameActionHandler<TGameContext>
{
    private readonly IGameRepository<TGameContext> gameRepository;
    private readonly ILogger<GameActionHandler<TGameContext>> logger;

    public GameActionHandler(
        IGameRepository<TGameContext> gameRepository,
        ILogger<GameActionHandler<TGameContext>> logger)
    {
        this.gameRepository = gameRepository;
        this.logger = logger;
    }

    public async Task<OneOf<Success, ActionFailed>> HandleGameActionAsync(GameId gameId, IAction gameAction)
    {
        var result = await gameRepository.GetGameEngineAsync(gameId);
        if (result.TryPickT1(out _, out var gameEngine))
        {
            return new ActionFailed("The game was not found.");
        }

        try
        {
            return await gameEngine.HandleActionAsync(gameAction);
        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Error during handling game action.");
            return new ActionFailed("Failed to process your action.");
        }
    }
}
