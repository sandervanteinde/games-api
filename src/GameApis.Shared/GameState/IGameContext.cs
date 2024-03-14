namespace GameApis.Shared.GameState;

public interface IGameContext<TSelf>
    where TSelf : IGameContext<TSelf>
{
    static abstract string GameIdentifier { get; }
    static abstract IGameState<TSelf> GetInitialState();
}