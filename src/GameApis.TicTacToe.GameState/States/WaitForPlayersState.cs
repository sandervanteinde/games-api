using GameApis.Shared.Dtos;
using GameApis.Shared.GameState;
using GameApis.TicTacToe.GameState.Actions;
using OneOf;
using OneOf.Types;

namespace GameApis.TicTacToe.GameState.States;

public class WaitForPlayersState
    : IGameState<TicTacToeContext>
    , IHandleGameAction<TicTacToeContext, JoinPlayerAction>
    , IHandleGameAction<TicTacToeContext, StartGameAction>
{
    public string GetDescription(TicTacToeContext gameContext)
    {
        return gameContext switch
        {
            { PlayerUsingX: null } => "Waiting for player one to join.",
            { PlayerUsingO: null } => "Waiting for player two to join.",
            _ => "Waiting for game to start"
        };
    }

    public OneOf<Success, ActionFailed> HandleAction(ActionContext<TicTacToeContext, JoinPlayerAction> actionContext)
    {
        var context = actionContext.Context;
        var playerId = actionContext.PlayerPerformingAction.ExternalId;
        if (context.PlayerUsingX is null)
        {
            context.PlayerUsingX = playerId;
            return new Success();
        }
        if (context.PlayerUsingX == playerId)
        {
            return AlreadyInLobby();
        }
        if (context.PlayerUsingO is null)
        {
            context.PlayerUsingO = playerId;
            return new Success();
        }
        if (context.PlayerUsingO == playerId)
        {
            return AlreadyInLobby();
        }

        return new ActionFailed("The lobby is full.");

        static ActionFailed AlreadyInLobby() => new ActionFailed("Player is already in the game lobby.");
    }

    public OneOf<Success, ActionFailed> HandleAction(ActionContext<TicTacToeContext, StartGameAction> actionContext)
    {
        var context = actionContext.Context;
        if (context is { PlayerUsingX: not null, PlayerUsingO: not null })
        {
            var playerStarting = actionContext.Action.PlayerStarting
                ?? (Random.Shared.Next(2) == 0
                    ? Models.PlayerTurn.PlayerX
                    : Models.PlayerTurn.PlayerO);
            context.PlayerTurn = playerStarting;
            actionContext.Engine.SetState<PlayerTurnState>();
            return new Success();
        }

        return new ActionFailed("There are not enough players.");
    }
}
