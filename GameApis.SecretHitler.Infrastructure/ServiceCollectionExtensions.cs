using GameApis.SecretHitler.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameApis.SecretHitler.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecretHitlerInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISecretHitlerGameService, SecretHitlerGameService>();
        return services;
    }
}
