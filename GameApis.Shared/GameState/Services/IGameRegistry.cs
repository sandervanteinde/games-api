using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

public interface IGameRegistry
{
    OneOf<Type, NotFound> GetGameStateForGameContext<GameContext>(string name);
    void RegisterGame(Type gameContext, string identifier, Type initialState);
    void RegisterGameState(Type gameContext, Type gameState);
    void RegisterGameAction(Type gameContext, Type gameAction);

    IEnumerable<GameRegistryEntry> EnumerateGameRegistryEntries();
}