using GameApis.Shared.GameState;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Services;

internal class GameStateResolver<TGameContext> : IGameStateResolver<TGameContext>
{
    private readonly IServiceProvider serviceProvider;
    private readonly IGameStateRegistry gameStateRegistry;

    public GameStateResolver(
        IServiceProvider serviceProvider,
        IGameStateRegistry gameStateRegistry
    )
    {
        this.serviceProvider = serviceProvider;
        this.gameStateRegistry = gameStateRegistry;
    }

    public OneOf<IGameState<TGameContext>, NotFound> ResolveGameState(string gameStateName)
    {
        var gameStateTypeResponse = gameStateRegistry.GetGameStateForGameContext<TGameContext>(gameStateName);
        if (gameStateTypeResponse.TryPickT1(out var notFound, out var gameStateType))
        {
            return notFound;
        }

        var gameState = (IGameState<TGameContext>)serviceProvider.GetRequiredService(gameStateType);

        return OneOf<IGameState<TGameContext>, NotFound>.FromT0(gameState);
    }
}
