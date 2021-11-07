using GameApis.Domain.Core;
using GameApis.SecretHitler.Domain.Entities;

namespace GameApis.SecretHitler.Domain.Events;

public class VoteResults : DomainEvent
{
    public Guid GameId { get; init; }
    public VoteResult[] Results { get; init; } = null!;

    public class VoteResult
    {
        public Guid ExternalPlayerId { get; init; }
        public Vote Vote { get; init; }
    }
}
