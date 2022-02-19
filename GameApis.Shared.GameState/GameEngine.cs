namespace GameApis.Shared.GameState;

public class GameEngine
{
    private IGameState gameState;

    public GameEngine(IGameState gameState)
    {
        this.gameState = gameState;
    }

    public async Task SetStateAsync<TNewState>()
        where TNewState : IGameState, new()
    {
        var newState = new TNewState();
        if(gameState is IOnLeaveActiveGameState onLeave)
        {
            await onLeave.OnLeaveActiveGameStateAsync(newState);
        }

        gameState = newState;

    }

    public static GameEngine Create<TInitial>()
        where TInitial : IGameState, new()
    {
        var state = new TInitial();
        return new GameEngine(state);
    }
}
