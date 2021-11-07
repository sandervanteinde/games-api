using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Entities;
public struct PlayerId
{
    /// <summary>
    /// Used to identify the player. Only the player itself should have this Id.
    /// </summary>
    public Guid InternalId { get; init; }

    /// <summary>
    /// Used to identify this player to other players.
    /// </summary>
    public Guid ExternalId { get; init; }
}
public class Player : Entity<PlayerId>
{
    public string Name { get; internal set; }

    public Role Role { get; internal set; }

    public bool Alive { get; internal set; } = true;

    public Vote? CastVote { get; internal set; }

    public Player(Guid internalId, Guid externalId, string name)
        : base(new PlayerId { InternalId = internalId, ExternalId = externalId })
    {
        Name = name;
        Role = Role.Unassigned;
    }

    public static Player CreateNew(string player)
    {
        return new(Guid.NewGuid(), Guid.NewGuid(), player);
    }
}
