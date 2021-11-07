using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class VoteSucceeded : DomainEvent
{
    public Guid GameId { get; init; }
}