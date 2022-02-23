using GameApis.Shared.GameState;
using GameApis.TicTacToe.GameState.Models;
using System.ComponentModel;

namespace GameApis.TicTacToe.GameState.States;

internal class GameFinishedState : IGameState<TicTacToeContext>
{
    public string GetDescription(TicTacToeContext gameContext)
    {
        return gameContext.Winner switch
        {
            PlayerTurn.None => "It is a draw",
            PlayerTurn.PlayerO => "O wins",
            PlayerTurn.PlayerX => "X wins",
            _ => throw new InvalidEnumArgumentException(nameof(gameContext.Winner), (int)gameContext.Winner, typeof(PlayerTurn))
        };
    }
}
