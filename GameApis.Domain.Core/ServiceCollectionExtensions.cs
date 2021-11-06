using GameApis.Domain.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace GameApis.Domain;

public static class ServiceCollectionExtensions
{
    private static readonly EventRegistry _eventRegistry = new();
    public static IServiceCollection AddDomainServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        var typeofDomainEvent = typeof(DomainEvent);
        var domainEvents = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type != typeofDomainEvent && type.IsAssignableTo(typeofDomainEvent));

        foreach (var domainEvent in domainEvents)
        {
            _eventRegistry.RegisterEventType(domainEvent.Name, domainEvent);
        }
        services.TryAddSingleton<IEventRegistry>(_eventRegistry);
        return services;
    }
}
