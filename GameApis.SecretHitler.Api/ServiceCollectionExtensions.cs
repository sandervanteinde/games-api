using GameApis.SecretHitler.Domain;
using GameApis.SecretHitler.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace GameApis.SecretHitler.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecretHitlerApi(this IServiceCollection services)
    {
        services.AddSecretHitlerInfrastructure();
        services.AddSecretHitlerDomain();
        return services;
    }
}
