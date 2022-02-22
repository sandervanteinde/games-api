namespace GameApis.Shared.Players;

/// <summary>
/// The internal player id. Can be used by an entity to identify itself as an entity.
/// </summary>
public record struct InternalPlayerId(Guid Value)
{
    public static InternalPlayerId New()
    {
        return new(Guid.NewGuid());
    }
}
