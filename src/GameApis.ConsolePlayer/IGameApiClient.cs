using GameApis.Shared.Players;
using Refit;

namespace GameApis.ConsolePlayer;

public record CreatePlayer(string PlayerName); 
public interface IGameApiClient
{
    [Post("/api/player")]
    Task<PlayerId> CreatePlayer(CreatePlayer playerName);
}