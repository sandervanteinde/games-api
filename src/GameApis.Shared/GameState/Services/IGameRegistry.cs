using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

public interface IGameRegistry
{
    OneOf<Type, NotFound> GetGameStateForGameContext<TContext>(string name);
    IEnumerable<GameRegistryEntry> EnumerateGameRegistryEntries();
}