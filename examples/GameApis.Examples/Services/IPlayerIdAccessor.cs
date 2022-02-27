using GameApis.Examples.Client;

namespace GameApis.Examples.Services;

public interface IPlayerIdAccessor
{
    Task<PlayerId?> GetPlayerIdAsync();
    Task SetPlayerIdAsync(PlayerId playerId);
    PlayerId? GetPlayerId();
}
