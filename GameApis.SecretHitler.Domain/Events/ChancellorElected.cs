using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class ChancellorElected : DomainEvent
{
    public Guid GameId { get; init; }
    public Guid ChancellorId { get; init; }
}
