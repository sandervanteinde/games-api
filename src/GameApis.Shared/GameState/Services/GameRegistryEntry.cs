namespace GameApis.Shared.GameState.Services;

public record GameRegistryEntry(
    Type GameContextType,
    string Identifier,
    Dictionary<string, Type> GameStateTypesByTypeName,
    List<Type> Actions);