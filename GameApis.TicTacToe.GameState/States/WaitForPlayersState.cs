using GameApis.Shared.GameState;
using GameApis.TicTacToe.GameState.Actions;

namespace GameApis.TicTacToe.GameState.States;

public class WaitForPlayersState
    : IGameState<TicTacToeContext>
    , IHandleGameAction<TicTacToeContext, JoinPlayerAction>
{
    public string GetDescription(TicTacToeContext gameContext)
    {
        return gameContext switch
        {
            { PlayerOneId: null } => "Waiting for player one to join.",
            { PlayerTwoId: null } => "Waiting for player two to join.",
            _ => "Waiting for game to start"
        };
    }

    public Task<ActionResult> HandleActionAsync(ActionContext<TicTacToeContext, JoinPlayerAction> actionContext)
    {
        var context = actionContext.Context;
        var playerId = actionContext.Action.PlayerId;
        if (context.PlayerOneId is null)
        {
            context.PlayerOneId = playerId;
            return ActionResult.Success();
        }
        if (context.PlayerOneId == playerId)
        {
            return AlreadyInLobby();
        }
        if (context.PlayerTwoId is null)
        {
            context.PlayerTwoId = playerId;
            return ActionResult.Success();
        }
        if (context.PlayerTwoId == playerId)
        {
            return AlreadyInLobby();
        }

        return ActionResult.Fail("The lobby is full.");

        static Task<ActionResult> AlreadyInLobby() => ActionResult.Fail("Player is already in the game lobby.");
    }
}
