using GameApis.Shared.Dtos;
using GameApis.Shared.Players;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState;

public class GameEngine<TGameContext>
    where TGameContext : IGameContext
{
    public TGameContext GameContext { get; }
    public IGameState<TGameContext> GameState { get; private set; }

    public GameEngine(
        IGameState<TGameContext> gameState,
        TGameContext gameContext
    )
    {
        ArgumentNullException.ThrowIfNull(gameState);
        ArgumentNullException.ThrowIfNull(gameContext);

        GameState = gameState;
        GameContext = gameContext;
    }

    internal async Task<OneOf<Success, ActionFailed>> HandleActionAsync<TAction>(PlayerId playerId, TAction action)
        where TAction : IAction
    {
        var actionContext = new ActionContext<TGameContext, TAction>(playerId, action, GameContext, this);
        if (GameState is IHandleGameAction<TGameContext, TAction> handler)
        {
            return handler.HandleAction(actionContext);
        }
        if (GameState is IHandleGameActionAsync<TGameContext, TAction> asyncHandler)
        {
            return await asyncHandler.HandleActionAsync(actionContext);
        }

        return new ActionFailed("The action was not valid in this context.");
    }

    public async Task SetStateAsync<TNewState>()
        where TNewState : IGameState<TGameContext>, new()
    {
        var newState = new TNewState();
        if (GameState is IOnLeaveActiveGameState<TGameContext> onLeave)
        {
            await onLeave.OnLeaveActiveGameStateAsync(newState, GameContext);
        }

        GameState = newState;
    }
}
