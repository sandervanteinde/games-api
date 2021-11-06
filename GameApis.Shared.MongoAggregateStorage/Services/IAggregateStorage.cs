using GameApis.Domain.Core;

namespace GameApis.Shared.MongoAggregateStorage.Services;

public interface IAggregateStorage<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : struct
{
    Task<IEnumerable<DomainEvent>> GetDomainEventsAsync(TId id);
    Task SaveEventsAsync(TEntity entity);
}
