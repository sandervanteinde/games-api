using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;

namespace GameApis.MongoDb.Conventions;

internal class DictionaryRepresentationConvention(DictionaryRepresentation dictionaryRepresentation) : ConventionBase, IMemberMapConvention
{
    public void Apply(BsonMemberMap memberMap)
    {
        memberMap.SetSerializer(ConfigureSerializer(memberMap.GetSerializer()));
    }

    private IBsonSerializer ConfigureSerializer(IBsonSerializer serializer)
    {
        if (serializer is IDictionaryRepresentationConfigurable dictionaryRepresentationConfigurable)
        {
            serializer = dictionaryRepresentationConfigurable.WithDictionaryRepresentation(dictionaryRepresentation);
        }

        return serializer is not IChildSerializerConfigurable childSerializerConfigurable
            ? serializer
            : childSerializerConfigurable.WithChildSerializer(ConfigureSerializer(childSerializerConfigurable.ChildSerializer));
    }
}