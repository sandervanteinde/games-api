using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class VoteFailed : DomainEvent
{
    public Guid GameId { get; init; }
}
