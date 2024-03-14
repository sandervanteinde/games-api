using GameApis.Shared.Dtos;
using GameApis.Shared.Players;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState;

public class GameEngine<TGameContext>
    where TGameContext : IGameContext<TGameContext>
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

    public void SetState<TNewState>()
        where TNewState : IGameState<TGameContext>, new()
    {
        var newState = new TNewState();
        GameState = newState;
    }

    internal async Task<OneOf<Success, ActionFailed>> HandleActionAsync<TAction>(PlayerId playerId, TAction action)
        where TAction : IAction
    {
        var actionContext = new ActionContext<TGameContext, TAction>(playerId, action, GameContext, this);

        return GameState switch
        {
            IHandleGameAction<TGameContext, TAction> handler => handler.HandleAction(actionContext),
            IHandleGameActionAsync<TGameContext, TAction> asyncHandler => await asyncHandler.HandleActionAsync(actionContext),
            _ => new ActionFailed("The action was not valid in this context.")
        };
    }
}