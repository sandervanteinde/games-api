using GameApis.Shared.GameState;
using GameApis.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameApis.Shared;

public class GameApiBuilder
{
    private readonly GameStateRegistry gameStateRegistry;
    private readonly IServiceCollection serviceCollection;

    internal GameApiBuilder(GameStateRegistry gameStateRegistry, IServiceCollection serviceCollection)
    {
        this.gameStateRegistry = gameStateRegistry;
        this.serviceCollection = serviceCollection;
    }

    public GameApiBuilder RegisterGameContext<TGameContext>()
        where TGameContext : IGameContext
    {
        var gameContextType = typeof(TGameContext);
        var gameStates = typeof(IGameState<>).MakeGenericType(gameContextType);

        foreach (var assemblyType in gameContextType.Assembly.DefinedTypes)
        {
            if (assemblyType.IsAssignableTo(gameStates))
            {
                gameStateRegistry.RegisterGameState(gameContextType, assemblyType);
                serviceCollection.AddTransient(assemblyType);
            }
        }

        return this;
    }
}