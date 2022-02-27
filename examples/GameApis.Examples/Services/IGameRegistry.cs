using GameApis.Examples.Models;

namespace GameApis.Examples.Services;

public interface IGameRegistry
{
    IEnumerable<GameEntry> GetAllGames();
}