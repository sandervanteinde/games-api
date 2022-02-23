using GameApis.Shared.Attributes;
using GameApis.Shared.GameState;
using GameApis.Shared.Players;
using GameApis.TicTacToe.GameState.Models;
using GameApis.TicTacToe.GameState.States;

namespace GameApis.TicTacToe.GameState;

[Game("tic-tac-toe", typeof(WaitForPlayersState))]
public class TicTacToeContext : IGameContext
{
    public ExternalPlayerId? PlayerUsingX { get; set; }
    public ExternalPlayerId? PlayerUsingO { get; set; }
    public PlayerTurn PlayerTurn { get; set; } = PlayerTurn.None;
    public Dictionary<BoardPositions, BoardState> PlayedPositions { get; set; } = new();
    public PlayerTurn Winner { get; set; } = PlayerTurn.None;
}