using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Players.Services;

public interface IInternalPlayerIdResolver
{
    Task<OneOf<InternalPlayerId, NotFound>> ResolveInternalPlayerIdAsync();
}
