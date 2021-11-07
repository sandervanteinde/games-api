using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class PlayerJoined : DomainEvent
{
    public Guid GameId { get; init; }
    public Guid ExternalPlayerId { get; init; }
    public Guid InternalPlayerId { get; init; }
    public string PlayerName { get; init; } = null!;
}
