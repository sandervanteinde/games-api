using StronglyTypedIds;

namespace GameApis.Shared;

[StronglyTypedId(converters: StronglyTypedIdConverter.SystemTextJson, backingType: StronglyTypedIdBackingType.Guid)]
public partial struct GameId { }
