using MongoDB.Bson.Serialization;

namespace GameApis.MongoDb.Serializers;

internal static class DefaultMongoSerializers
{
    public static readonly IBsonSerializer<Guid> GuidSerializer = BsonSerializer.SerializerRegistry.GetSerializer<Guid>();
}
