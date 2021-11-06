using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class PlayerJoined : DomainEvent
{
    public Guid GameId { get; init; }
    public Guid PlayerId { get; init; }
    public string PlayerName { get; init; }
}
