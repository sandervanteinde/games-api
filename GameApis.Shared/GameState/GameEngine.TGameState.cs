using GameApis.Shared.Dtos;
using GameApis.Shared.Players;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState;

public class GameEngine<TGameContext>
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
        if (GameState is not IHandleGameAction<TGameContext, TAction> handler)
        {
            return new ActionFailed("The action was invaid in the current game state");
        }

        var actionContext = new ActionContext<TGameContext, TAction>(playerId, action, GameContext, this);
        await handler.HandleActionAsync(actionContext);
        return new Success();

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
