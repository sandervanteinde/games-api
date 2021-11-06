using GameApis.Domain.Core;
using GameApis.Shared.CancellationTokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Data;

namespace GameApis.Shared.MongoAggregateStorage.Services;

internal class MongoAggregateStorage<TEntity, TId> : IAggregateStorage<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : struct
{
    private readonly IMongoClient mongoClient;
    private readonly ICancellationTokenProvider cancellationTokenProvider;
    private readonly IEventRegistry eventRegistry;

    public MongoAggregateStorage(
        IMongoClient mongoClient,
        ICancellationTokenProvider cancellationTokenProvider,
        IEventRegistry eventRegistry
    )
    {
        this.mongoClient = mongoClient;
        this.cancellationTokenProvider = cancellationTokenProvider;
        this.eventRegistry = eventRegistry;
    }

    public async Task<IEnumerable<DomainEvent>> GetDomainEventsAsync(TId id)
    {
        var db = mongoClient.GetDatabase(DatabaseName());
        var collection = db.GetCollection<BsonDocument>(id.ToString());
        var bsonDocuments = await collection.AsQueryable().ToListAsync(cancellationTokenProvider.CancellationToken);
        var result = new List<DomainEvent>(bsonDocuments.Count);
        foreach (var document in bsonDocuments)
        {
            var typeName = document["_t"].AsString;
            document.Remove("_id");
            var eventType = eventRegistry.GetEventType(typeName);
            var asDomainEvent = (DomainEvent)BsonSerializer.Deserialize(document, eventType);
            result.Add(asDomainEvent);
        }

        return result;
    }

    public async Task SaveEventsAsync(TEntity entity)
    {
        if (entity.Version == entity.OriginalVersion)
        {
            return;
        }

        var db = mongoClient.GetDatabase(DatabaseName());
        var collection = db.GetCollection<DomainEvent>(entity.Id.ToString());
        var eventCount = await collection.CountDocumentsAsync(Builders<DomainEvent>.Filter.Empty, cancellationToken: cancellationTokenProvider.CancellationToken);
        if (eventCount != entity.OriginalVersion)
        {
            throw new DBConcurrencyException();
        }
        var events = entity.GetEvents();
        await collection.InsertManyAsync(events, cancellationToken: cancellationTokenProvider.CancellationToken);
    }

    private static string DatabaseName()
    {
        return typeof(TEntity).Name;
    }
}
