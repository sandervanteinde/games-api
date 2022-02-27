using GameApis.MongoDb.Conventions;
using GameApis.MongoDb.Serializers;
using GameApis.Shared.GameState.Services;
using GameApis.Shared.Players;
using GameApis.Shared.Players.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;

namespace GameApis.MongoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameApisMongoDbPersistence(this IServiceCollection services, string connectionString)
    {
        // Set up MongoDB conventions
        var pack = new ConventionPack
        {
            new EnumRepresentationConvention(BsonType.String),
            new DictionaryRepresentationConvention(DictionaryRepresentation.ArrayOfDocuments)
        };
        ConventionRegistry.Register("EnumStringConvention", pack, _ => true);

        BsonSerializer.RegisterSerializer(new GameIdSerializer());
        BsonSerializer.RegisterSerializer(new ExternalPlayerIdSerializer());
        BsonSerializer.RegisterSerializer(new InternalPlayerIdSerializer());

        BsonClassMap.RegisterClassMap<Player>(opts =>
        {
            opts.MapIdMember(player => player.Id);
        });

        services.TryAddSingleton<IMongoClient>(_ =>
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(connectionString);
            return new MongoClient(mongoClientSettings);
        });

        services.AddTransient(typeof(IGameRepository<>), typeof(MongoGameRepository<>));
        services.AddTransient<IPlayerRepository, PlayerRepository>();

        return services;
    }
}
