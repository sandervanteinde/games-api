using GameApis.Examples.TicTacToe.Client;

namespace GameApis.Examples.TicTacToe.Services;

public interface IPlayerIdAccessor
{
    bool TryGetPlayerId(out PlayerId playerId);
    void SetPlayerId(PlayerId playerId);
}
