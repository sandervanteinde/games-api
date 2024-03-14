using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

internal class GameRegistry : IGameRegistry
{
    private readonly Dictionary<Type, GameRegistryEntry> registryPerGameContext = new();

    public OneOf<Type, NotFound> GetGameStateForGameContext<TContext>(string name)
    {
        var gameContextType = typeof(TContext);

        if (!registryPerGameContext.TryGetValue(gameContextType, out var gameRegistryEntry))
        {
            return new NotFound();
        }

        if (!gameRegistryEntry.GameStateTypesByTypeName.TryGetValue(name, out var stateType))
        {
            return new NotFound();
        }

        return stateType;
    }

    public void RegisterGameState(Type gameContext, Type gameState)
    {
        var gameRegistryEntry = GetGameRegistryEntryForGameContext(gameContext);
        gameRegistryEntry.GameStateTypesByTypeName.Add(gameState.Name, gameState);
    }

    public void RegisterGameAction(Type gameContext, Type gameAction)
    {
        var gameRegistryEntry = GetGameRegistryEntryForGameContext(gameContext);
        gameRegistryEntry.Actions.Add(gameAction);
    }

    public void RegisterGame(Type gameContext, string identifier)
    {
        registryPerGameContext[gameContext] = new GameRegistryEntry(gameContext, identifier, new Dictionary<string, Type>(), []);
    }

    public IEnumerable<GameRegistryEntry> EnumerateGameRegistryEntries()
    {
        return registryPerGameContext.Values;
    }

    private GameRegistryEntry GetGameRegistryEntryForGameContext(Type gameContext)
    {
        return registryPerGameContext[gameContext];
    }
}