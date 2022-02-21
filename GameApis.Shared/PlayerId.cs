namespace GameApis.Shared;

public struct PlayerId : IEquatable<PlayerId>
{
    public Guid InternalId { get; set; }
    public Guid ExternalId { get; set; }

    public bool Equals(PlayerId other)
    {
        return InternalId == other.InternalId
            && ExternalId == other.ExternalId;
    }
    public static bool operator ==(PlayerId left, PlayerId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PlayerId left, PlayerId right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(InternalId, ExternalId);
    }

    public override bool Equals(object? obj)
    {
        return obj is PlayerId playerId && Equals(playerId);
    }
}
