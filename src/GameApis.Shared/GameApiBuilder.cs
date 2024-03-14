using GameApis.Shared.GameState;
using GameApis.Shared.GameState.Services;
using Microsoft.Extensions.DependencyInjection;

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
        where TGameContext : IGameContext<TGameContext>
    {
        var gameContextType = typeof(TGameContext);

        gameStateRegistry.RegisterGame(gameContextType, TGameContext.GameIdentifier);

        var gameStates = typeof(IGameState<>)
            .MakeGenericType(gameContextType);

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