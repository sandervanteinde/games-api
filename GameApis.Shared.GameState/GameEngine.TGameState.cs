namespace GameApis.Shared.GameState;

public class GameEngine<TGameContext>
{
    private IGameState<TGameContext> gameState;
    private TGameContext gameContext;

    public GameEngine(
        IGameState<TGameContext> gameState,
        TGameContext gameContext
    )
    {
        this.gameState = gameState;
        this.gameContext = gameContext;
    }

    public async Task SetStateAsync<TNewState>()
        where TNewState : IGameState<TGameContext>, new()
    {
        var newState = new TNewState();
        if(gameState is IOnLeaveActiveGameState<TGameContext> onLeave)
        {
            await onLeave.OnLeaveActiveGameStateAsync(newState, gameContext);
        }

        gameState = newState;
    }
}
