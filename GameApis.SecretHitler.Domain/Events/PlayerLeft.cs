using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class PlayerLeft : DomainEvent
{
    public Guid GameId { get; init; }
    public Guid PlayerId { get; init; }
}
