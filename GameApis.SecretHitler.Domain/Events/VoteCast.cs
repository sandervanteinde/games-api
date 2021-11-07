using GameApis.Domain.Core;
using GameApis.SecretHitler.Domain.Entities;

namespace GameApis.SecretHitler.Domain.Events;

public class VoteCast : DomainEvent
{
    public Guid GameId {  get; init; }
    public Guid PlayerId { get; init; }
    public Vote Vote { get; init; }
}
