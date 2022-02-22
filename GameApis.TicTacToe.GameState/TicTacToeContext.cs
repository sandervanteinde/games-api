using GameApis.Shared.Attributes;
using GameApis.Shared.GameState;
using GameApis.Shared.Players;
using GameApis.TicTacToe.GameState.States;

namespace GameApis.TicTacToe.GameState;

[Game("tic-tac-toe", typeof(WaitForPlayersState))]
public class TicTacToeContext : IGameContext
{
    public ExternalPlayerId? PlayerOneId { get; set; }
    public ExternalPlayerId? PlayerTwoId { get; set; }
}
