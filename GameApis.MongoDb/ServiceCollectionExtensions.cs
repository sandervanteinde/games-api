using GameApis.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace GameApis.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameApisMongoDbPersistence(this IServiceCollection services, string connectionString)
    {
        // Set up MongoDB conventions
        var pack = new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String)
        };
        ConventionRegistry.Register("EnumStringConvention", pack, t => true);

        services.TryAddSingleton<IMongoClient>(sp =>
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            return new MongoClient(mongoClientSettings);
        });

        services.AddTransient(typeof(IGameRepository<>), typeof(MongoGameRepository<>));

        return services;
    }
}
