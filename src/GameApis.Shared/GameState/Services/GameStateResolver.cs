using Microsoft.Extensions.DependencyInjection;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

internal class GameStateResolver<TGameContext>(
    IServiceProvider serviceProvider,
    IGameRegistry gameStateRegistry) : IGameStateResolver<TGameContext>
{
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