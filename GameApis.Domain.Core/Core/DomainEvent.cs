namespace GameApis.Domain.Core;

public abstract class DomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
}
