using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Events;

public class GameStarted : DomainEvent
{
    public Guid GameId { get; init; }
}
