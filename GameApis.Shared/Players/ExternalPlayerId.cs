namespace GameApis.Shared.Players;

/// <summary>
/// The external player id. Can be used to reference a player entity.
/// </summary>
public record struct ExternalPlayerId(Guid Value)
{
    public static ExternalPlayerId New()
    {
        return new(Guid.NewGuid());
    }
}
