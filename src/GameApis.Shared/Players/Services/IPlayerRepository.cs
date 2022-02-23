using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Players.Services;

public interface IPlayerRepository
{
    Task<OneOf<Player, NotFound>> GetPlayerByInternalIdAsync(InternalPlayerId internalId);
    Task StorePlayerAsyc(Player player);
}
