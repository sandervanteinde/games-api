using GameApis.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace GameApis.SecretHitler.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecretHitlerDomain(this IServiceCollection services)
    {
        services.AddDomainServices(typeof(ServiceCollectionExtensions).Assembly);
        return services;
    }
}
