using GameApis.Shared.Players;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameApis.MongoDb.Serializers;

internal class ExternalPlayerIdSerializer : SerializerBase<ExternalPlayerId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ExternalPlayerId value)
    {
        DefaultMongoSerializers.GuidSerializer.Serialize(context, args, value.Value);
    }

    public override ExternalPlayerId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var guid = DefaultMongoSerializers.GuidSerializer.Deserialize(context, args);
        return new ExternalPlayerId(guid);
    }
}
