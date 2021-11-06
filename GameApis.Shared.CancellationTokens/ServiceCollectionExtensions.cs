using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GameApis.Shared.CancellationTokens;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHttpContextCancellationTokenPassing(this IServiceCollection services)
    {
        services.AddScoped<ICancellationTokenProvider, HttpContextCancellationTokenProvider>();
        return services;
    }

    public static IServiceCollection AddFallbackCancellationTokenPassing(this IServiceCollection services)
    {
        services.TryAddScoped<ICancellationTokenProvider, NoneCancellationTokenProvider>();
        return services;
    }
}
