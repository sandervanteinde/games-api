using GameApis.Shared.Dtos;
using GameApis.Shared.GameState;
using GameApis.TicTacToe.GameState.Actions;
using OneOf;
using OneOf.Types;

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

    public OneOf<Success, ActionFailed> HandleAction(ActionContext<TicTacToeContext, JoinPlayerAction> actionContext)
    {
        var context = actionContext.Context;
        var playerId = actionContext.PlayerPerformingAction.ExternalId;
        if (context.PlayerOneId is null)
        {
            context.PlayerOneId = playerId;
            return new Success();
        }
        if (context.PlayerOneId == playerId)
        {
            return AlreadyInLobby();
        }
        if (context.PlayerTwoId is null)
        {
            context.PlayerTwoId = playerId;
            return new Success();
        }
        if (context.PlayerTwoId == playerId)
        {
            return AlreadyInLobby();
        }

        return new ActionFailed("The lobby is full.");

        static ActionFailed AlreadyInLobby() => new ActionFailed("Player is already in the game lobby.");
    }
}
