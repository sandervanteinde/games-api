using GameApis.Shared.Attributes;
using GameApis.Shared.GameState;
using GameApis.Shared.GameState.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GameApis.Shared;

public class GameApiBuilder
{
    private readonly GameRegistry gameStateRegistry;
    private readonly IServiceCollection serviceCollection;

    internal GameApiBuilder(GameRegistry gameStateRegistry, IServiceCollection serviceCollection)
    {
        this.gameStateRegistry = gameStateRegistry;
        this.serviceCollection = serviceCollection;
    }

    public GameApiBuilder RegisterGameContext<TGameContext>()
        where TGameContext : IGameContext
    {
        var gameContextType = typeof(TGameContext);
        var gameAttribute = gameContextType.GetCustomAttribute<GameAttribute>();
        if (gameAttribute is null)
        {
            throw new InvalidOperationException("A [Game] attribute is required on the game context.");
        }

        gameStateRegistry.RegisterGame(gameContextType, gameAttribute.Identifier, gameAttribute.InitialState);

        var gameStates = typeof(IGameState<>).MakeGenericType(gameContextType);
        var gameAction = typeof(IAction);

        foreach (var assemblyType in gameContextType.Assembly.DefinedTypes)
        {
            if (assemblyType.IsAssignableTo(gameStates))
            {
                gameStateRegistry.RegisterGameState(gameContextType, assemblyType);
                serviceCollection.AddTransient(assemblyType);
            }
            if (assemblyType.IsAssignableTo(gameAction))
            {
                gameStateRegistry.RegisterGameAction(gameContextType, assemblyType);
            }
        }

        return this;
    }
}