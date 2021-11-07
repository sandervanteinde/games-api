using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Entities;

public class Player : Entity<Guid>
{
    public string Name { get; internal set; }

    public Role Role { get; internal set; }

    public bool Alive { get; internal set; } = true;

    public Player(Guid id, string name)
        : base(id)
    {
        Name = name;
        Role = Role.Unassigned;
    }

    public static Player CreateNew(string player)
    {
        return new(Guid.NewGuid(), player);
    }
}
