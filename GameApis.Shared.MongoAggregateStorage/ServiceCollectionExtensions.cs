using GameApis.Shared.MongoAggregateStorage.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace GameApis.Shared.MongoAggregateStorage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAggregateMongoStorage(this IServiceCollection services, string connectionString)
    {
        // Set up MongoDB conventions
        var pack = new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        };
        ConventionRegistry.Register("EnumStringConvention", pack, t => true);

        services.TryAddScoped(typeof(IAggregateStorage<,>), typeof(MongoAggregateStorage<,>));
        services.TryAddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
        return services;
    }
}
