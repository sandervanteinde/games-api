namespace GameApis.Shared.GameState;

public interface IGameState
{

}

public interface IOnLeaveActiveGameState
{
    Task OnLeaveActiveGameStateAsync(IGameState newGameState);
}
