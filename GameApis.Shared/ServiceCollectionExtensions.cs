using GameApis.Shared.GameState.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameApis.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameApisServices(this IServiceCollection services, Action<GameApiBuilder> gameApiBuilder)
    {
        services.AddTransient(typeof(IGameActionHandler<>), typeof(GameActionHandler<>));
        services.AddTransient(typeof(IGameStateResolver<>), typeof(GameStateResolver<>));

        var gameStateRegistry = new GameRegistry();
        var builder = new GameApiBuilder(gameStateRegistry, services);
        gameApiBuilder(builder);

        services.AddSingleton<IGameRegistry>(gameStateRegistry);
        return services;
    }
}
