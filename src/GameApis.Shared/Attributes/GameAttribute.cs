namespace GameApis.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class GameAttribute : Attribute
{
    public string Identifier { get; }
    public Type InitialState { get; }

    public GameAttribute(string identifier, Type initialState)
    {
        Identifier = identifier;
        InitialState = initialState;
    }

}
