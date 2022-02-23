namespace GameApis.Shared.Players;

/// <summary>
/// A player id consists out of set of two unique id's.
/// </summary>
/// <param name="InternalId">The internal id as known by the user itself. It should only be known to the entity who owns this player.</param>
/// <param name="ExternalId">The external id used by other players to reference this entity.</param>
public record struct PlayerId(InternalPlayerId InternalId, ExternalPlayerId ExternalId)
{
    public static PlayerId New()
    {
        return new(InternalPlayerId.New(), ExternalPlayerId.New());
    }

    public static bool operator ==(PlayerId playerId, InternalPlayerId internalPlayerId)
    {
        return playerId.InternalId == internalPlayerId;
    }

    public static bool operator ==(InternalPlayerId internalPlayerId, PlayerId playerId)
    {
        return playerId.InternalId == internalPlayerId;
    }

    public static bool operator !=(PlayerId playerId, InternalPlayerId internalPlayerId)
    {
        return playerId.InternalId != internalPlayerId;
    }

    public static bool operator !=(InternalPlayerId internalPlayerId, PlayerId playerId)
    {
        return playerId.InternalId != internalPlayerId;
    }

    public static bool operator ==(PlayerId playerId, ExternalPlayerId externalPlayerId)
    {
        return playerId.ExternalId == externalPlayerId;
    }

    public static bool operator ==(ExternalPlayerId externalPlayerId, PlayerId playerId)
    {
        return playerId.ExternalId == externalPlayerId;
    }

    public static bool operator !=(PlayerId playerId, ExternalPlayerId externalPlayerId)
    {
        return playerId.ExternalId != externalPlayerId;
    }

    public static bool operator !=(ExternalPlayerId externalPlayerId, PlayerId playerId)
    {
        return playerId.ExternalId != externalPlayerId;
    }

    public static bool operator ==(PlayerId playerId, InternalPlayerId? internalPlayerId)
    {
        return playerId.InternalId == internalPlayerId;
    }

    public static bool operator ==(InternalPlayerId? internalPlayerId, PlayerId playerId)
    {
        return playerId.InternalId == internalPlayerId;
    }

    public static bool operator !=(PlayerId playerId, InternalPlayerId? internalPlayerId)
    {
        return playerId.InternalId != internalPlayerId;
    }

    public static bool operator !=(InternalPlayerId? internalPlayerId, PlayerId playerId)
    {
        return playerId.InternalId != internalPlayerId;
    }

    public static bool operator ==(PlayerId playerId, ExternalPlayerId? externalPlayerId)
    {
        return playerId.ExternalId == externalPlayerId;
    }

    public static bool operator ==(ExternalPlayerId? externalPlayerId, PlayerId playerId)
    {
        return playerId.ExternalId == externalPlayerId;
    }

    public static bool operator !=(PlayerId playerId, ExternalPlayerId? externalPlayerId)
    {
        return playerId.ExternalId != externalPlayerId;
    }

    public static bool operator !=(ExternalPlayerId? externalPlayerId, PlayerId playerId)
    {
        return playerId.ExternalId != externalPlayerId;
    }
}
