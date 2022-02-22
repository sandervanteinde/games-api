using GameApis.Shared;
using GameApis.Shared.GameState;

namespace GameApis.TicTacToe.GameState;

public class TicTacToeContext : IGameContext
{
    public PlayerId? PlayerOneId { get; set; }
    public PlayerId? PlayerTwoId { get; set; }
}
