namespace GameApis.Domain.Core;

public abstract class Entity<TId>
{
    public TId Id { get; }

    public Entity(TId id)
    {
        Id = id;
    }
}
