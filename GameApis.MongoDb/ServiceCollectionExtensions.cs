using GameApis.MongoDb.Serializers;
using GameApis.Shared.GameState.Services;
using GameApis.Shared.Players;
using GameApis.Shared.Players.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

        BsonSerializer.RegisterSerializer(new GameIdSerializer());
        BsonSerializer.RegisterSerializer(new ExternalPlayerIdSerializer());
        BsonSerializer.RegisterSerializer(new InternalPlayerIdSerializer());

        BsonClassMap.RegisterClassMap<Player>(opts =>
        {
            opts.MapIdMember(player => player.Id);
        });

        services.TryAddSingleton<IMongoClient>(sp =>
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            return new MongoClient(mongoClientSettings);
        });

        services.AddTransient(typeof(IGameRepository<>), typeof(MongoGameRepository<>));
        services.AddTransient<IPlayerRepository, PlayerRepository>();

        return services;
    }
}
