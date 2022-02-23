﻿using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;

namespace GameApis.MongoDb.Conventions;

internal class DictionaryRepresentationConvention : ConventionBase, IMemberMapConvention
{
    private readonly DictionaryRepresentation _dictionaryRepresentation;
    public DictionaryRepresentationConvention(DictionaryRepresentation dictionaryRepresentation)
    {
        _dictionaryRepresentation = dictionaryRepresentation;
    }
    public void Apply(BsonMemberMap memberMap)
    {
        memberMap.SetSerializer(ConfigureSerializer(memberMap.GetSerializer()));
    }

    private IBsonSerializer ConfigureSerializer(IBsonSerializer serializer)
    {
        if (serializer is IDictionaryRepresentationConfigurable dictionaryRepresentationConfigurable)
        {
            serializer = dictionaryRepresentationConfigurable.WithDictionaryRepresentation(_dictionaryRepresentation);
        }

        return serializer is not IChildSerializerConfigurable childSerializerConfigurable
            ? serializer
            : childSerializerConfigurable.WithChildSerializer(ConfigureSerializer(childSerializerConfigurable.ChildSerializer));
    }
}
