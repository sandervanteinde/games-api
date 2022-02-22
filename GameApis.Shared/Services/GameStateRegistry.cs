using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Services;

internal class GameStateRegistry : IGameStateRegistry
{
    private readonly Dictionary<Type, Dictionary<string, Type>> registryPerGameContext = new();
    public OneOf<Type, NotFound> GetGameStateForGameContext<GameContext>(string name)
    {
        var gameContextType = typeof(GameContext);
        if (!registryPerGameContext.TryGetValue(gameContextType, out var gameContextStates))
        {
            return new NotFound();
        }

        if (!gameContextStates.TryGetValue(name, out var stateType))
        {
            return new NotFound();
        }

        return stateType;
    }

    public void RegisterGameState(Type gameContext, Type gameState)
    {
        if (!registryPerGameContext.TryGetValue(gameContext, out var gameContextStates))
        {
            registryPerGameContext[gameContext] = gameContextStates = new();
        }

        gameContextStates.Add(gameState.Name, gameState);
    }
}
