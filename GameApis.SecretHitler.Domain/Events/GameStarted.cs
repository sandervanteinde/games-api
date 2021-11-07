using GameApis.Domain.Core;
using GameApis.SecretHitler.Domain.Entities;

namespace GameApis.SecretHitler.Domain.Events;

public class GameStarted : DomainEvent
{
    public Guid GameId { get; init; }
    public RoleAssignment[] Assignments { get; init; }
    public Card[] Cards { get; init; } = null!;
    public Guid InitialPresidentId { get; init; }

    public class RoleAssignment
    {
        public Guid PlayerId { get; init; }
        public Role Role { get; init; }
    }
}
