using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class GameCreated : DomainEvent
{
    public Guid GameId { get; init; }
}
