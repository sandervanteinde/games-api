using GameApis.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameApis.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameApisServices(this IServiceCollection services, Action<GameApiBuilder> gameApiBuilder)
    {
        services.AddTransient(typeof(IGameActionHandler<>), typeof(GameActionHandler<>));
        services.AddTransient(typeof(IGameStateResolver<>), typeof(GameStateResolver<>));

        var gameStateRegistry = new GameStateRegistry();
        var builder = new GameApiBuilder(gameStateRegistry, services);
        gameApiBuilder(builder);

        services.AddSingleton<IGameStateRegistry>(gameStateRegistry);
        return services;
    }
}
