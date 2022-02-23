namespace GameApis.Shared.GameState;

public interface IOnLeaveActiveGameState<TGameContext>
{
    Task OnLeaveActiveGameStateAsync(IGameState<TGameContext> newGameState, TGameContext context);
}
