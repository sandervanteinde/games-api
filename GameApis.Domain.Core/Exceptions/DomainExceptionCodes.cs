namespace GameApis.Domain.Exceptions;

public enum DomainExceptionCodes
{
    LobbyFull,
    GameHasStarted,
    PlayerNotFound,
    NotEnoughPlayers,
    DuplicatePlayerName,
    PlayerCantPerformAction,
    InvalidTargetPlayer,
    ActionAlreadyPerformed
}
