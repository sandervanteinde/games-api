using GameApis.Domain.Core;

namespace GameApis.SecretHitler.Domain.Entities;

public class Player : Entity<Guid>
{
    public string Name { get; private set; }

    public Role Role { get; private set; }

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
