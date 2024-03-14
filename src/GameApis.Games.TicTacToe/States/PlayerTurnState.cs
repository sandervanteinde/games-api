using GameApis.Games.TicTacToe.Actions;
using GameApis.Games.TicTacToe.Models;
using GameApis.Shared.Dtos;
using GameApis.Shared.GameState;
using GameApis.Shared.Players;
using OneOf;
using OneOf.Types;
using System.ComponentModel;

namespace GameApis.Games.TicTacToe.States;

internal class PlayerTurnState
    : IGameState<TicTacToeContext>
        , IHandleGameAction<TicTacToeContext, PerformPlayAction>
{
    private static readonly BoardPositions[][] WinPositions =
    [
        [BoardPositions.TopLeft, BoardPositions.Top, BoardPositions.TopRight],
        [BoardPositions.Left, BoardPositions.Middle, BoardPositions.Right],
        [BoardPositions.BottomLeft, BoardPositions.Bottom, BoardPositions.BottomRight],
        [BoardPositions.TopLeft, BoardPositions.Left, BoardPositions.BottomLeft],
        [BoardPositions.Top, BoardPositions.Middle, BoardPositions.Bottom],
        [BoardPositions.TopRight, BoardPositions.Right, BoardPositions.BottomRight],
        [BoardPositions.TopLeft, BoardPositions.Middle, BoardPositions.BottomRight],
        [BoardPositions.TopRight, BoardPositions.Middle, BoardPositions.BottomLeft]
    ];

    public string GetDescription(TicTacToeContext gameContext)
    {
        return gameContext.PlayerTurn switch
        {
            PlayerTurn.PlayerX => "Player one's turn to make a move",
            PlayerTurn.PlayerO => "Player two's turn to make a move",
            _ => "Unknown state"
        };
    }

    public OneOf<Success, ActionFailed> HandleAction(ActionContext<TicTacToeContext, PerformPlayAction> actionContext)
    {
        actionContext.Deconstruct(out var player, out var action, out var context, out var engine);

        if (!IsItPlayersTurn(context, player, out var playerTurn))
        {
            return new ActionFailed("It is not your turn");
        }

        if (context.PlayedPositions.ContainsKey(action.Position))
        {
            return new ActionFailed("This position was already taken.");
        }

        context.PlayedPositions.Add(
            action.Position, playerTurn switch
            {
                PlayerTurn.PlayerO => BoardState.O,
                PlayerTurn.PlayerX => BoardState.X,
                _ => throw new InvalidEnumArgumentException(nameof(playerTurn), (int)playerTurn, typeof(PlayerTurn))
            }
        );

        context.PlayerTurn = context.PlayerTurn is PlayerTurn.PlayerO
            ? PlayerTurn.PlayerX
            : PlayerTurn.PlayerO;

        if (HasGameEnded(context, out var winner))
        {
            context.Winner = winner;
            engine.SetState<GameFinishedState>();
        }

        return new Success();
    }

    private static bool IsItPlayersTurn(TicTacToeContext context, PlayerId player, out PlayerTurn playerTurn)
    {
        if (context.PlayerUsingO == player && context.PlayerTurn == PlayerTurn.PlayerO)
        {
            playerTurn = PlayerTurn.PlayerO;
            return true;
        }

        if (context.PlayerUsingX == player && context.PlayerTurn == PlayerTurn.PlayerX)
        {
            playerTurn = PlayerTurn.PlayerX;
            return true;
        }

        playerTurn = default;
        return false;
    }

    private static bool HasGameEnded(TicTacToeContext context, out PlayerTurn winner)
    {
        foreach (var winPosition in WinPositions)
        {
            if (IsMatch(winPosition, out winner))
            {
                return true;
            }
        }

        if (context.PlayedPositions.Count >= 9)
        {
            winner = PlayerTurn.None;
            return true;
        }

        winner = default;
        return false;

        bool IsMatch(BoardPositions[] positions, out PlayerTurn winner)
        {
            BoardState stateFound = default;

            for (var i = 0; i < positions.Length; i++)
            {
                if (!context.PlayedPositions.TryGetValue(positions[i], out var state))
                {
                    winner = default;
                    return false;
                }

                if (i == 0)
                {
                    stateFound = state;
                }
                else if (stateFound != state)
                {
                    winner = default;
                    return false;
                }
            }

            winner = stateFound switch
            {
                BoardState.X => PlayerTurn.PlayerX,
                BoardState.O => PlayerTurn.PlayerO,
                _ => throw new InvalidOperationException("Invalid board state")
            };
            return true;
        }
    }
}