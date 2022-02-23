using GameApis.Shared.Players;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameApis.MongoDb.Serializers;

internal class InternalPlayerIdSerializer : SerializerBase<InternalPlayerId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, InternalPlayerId value)
    {
        DefaultMongoSerializers.GuidSerializer.Serialize(context, args, value.Value);
    }

    public override InternalPlayerId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var guid = DefaultMongoSerializers.GuidSerializer.Deserialize(context, args);
        return new InternalPlayerId(guid);
    }
}
