using GameApis.Examples.Models;

namespace GameApis.Examples.Services;

public class GameRegistry : IGameRegistry
{
    private readonly GameEntry[] _games =
    {
        new("tic-tac-toe", "Tic Tac Toe",
            "Classic game of Tic Tac Toe where the goal is to get 3 of your own symbol in one row.")
    };

    public IEnumerable<GameEntry> GetAllGames()
    {
        return _games;
    }
}