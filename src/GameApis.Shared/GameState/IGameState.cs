namespace GameApis.Shared.GameState;

public interface IGameState<TGameContext>
{
    string GetDescription(TGameContext gameContext);
}