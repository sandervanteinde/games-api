using GameApis.Shared.GameState;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameApis.MongoDb.Serializers;

internal class GameIdSerializer : SerializerBase<GameId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, GameId value)
    {
        DefaultMongoSerializers.GuidSerializer.Serialize(context, args, value.Value);
    }

    public override GameId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var guid = DefaultMongoSerializers.GuidSerializer.Deserialize(context, args);
        return new GameId(guid);
    }
}