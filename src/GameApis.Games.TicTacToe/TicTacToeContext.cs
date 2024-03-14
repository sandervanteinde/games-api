using GameApis.Games.TicTacToe.Models;
using GameApis.Games.TicTacToe.States;
using GameApis.Shared.GameState;
using GameApis.Shared.Players;

namespace GameApis.Games.TicTacToe;

public class TicTacToeContext : IGameContext<TicTacToeContext>
{
    public ExternalPlayerId? PlayerUsingX { get; set; }
    public ExternalPlayerId? PlayerUsingO { get; set; }
    public PlayerTurn PlayerTurn { get; set; } = PlayerTurn.None;
    public Dictionary<BoardPositions, BoardState> PlayedPositions { get; set; } = new();
    public PlayerTurn Winner { get; set; } = PlayerTurn.None;

    public static string GameIdentifier => "TicTacToe";

    public static IGameState<TicTacToeContext> GetInitialState()
    {
        return new WaitForPlayersState();
    }
}