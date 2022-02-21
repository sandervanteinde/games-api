using GameApis.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameApis.Shared;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameApisServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IGameActionHandler<>), typeof(GameActionHandler<>));
        return services;
    }
}
