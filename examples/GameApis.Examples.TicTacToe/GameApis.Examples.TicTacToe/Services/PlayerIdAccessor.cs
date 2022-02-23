using GameApis.Examples.TicTacToe.Client;

namespace GameApis.Examples.TicTacToe.Services;

public class PlayerIdAccessor : IPlayerIdAccessor
{
    private PlayerId? playerId;
    public void SetPlayerId(PlayerId playerId)
    {
        this.playerId = playerId;
    }

    public bool TryGetPlayerId(out PlayerId playerId)
    {
        if (this.playerId is null)
        {
            playerId = new();
            return false;
        }
        playerId = this.playerId;
        return true;
    }
}