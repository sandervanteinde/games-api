namespace GameApis.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class GameAttribute(string identifier, Type initialState) : Attribute
{
    public string Identifier { get; } = identifier;
    public Type InitialState { get; } = initialState;
}