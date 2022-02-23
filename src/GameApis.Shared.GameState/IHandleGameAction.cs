namespace GameApis.Shared.GameState;

public interface IHandleGameAction<TGameContext, TAction>
{
    Task<ActionResult> HandleActionAsync(ActionContext<TGameContext, TAction> actionContext);
}
