namespace GameApis.Shared.GameState;

public static class GameEngine
{
    public static GameEngine<TGameContext> Create<TInitialState, TGameContext>()
        where TGameContext : new()
        where TInitialState : IGameState<TGameContext>, new()
    {
        var state = new TInitialState();
        var gameContext = new TGameContext();
        return new GameEngine<TGameContext>(state, gameContext);
    }
}
