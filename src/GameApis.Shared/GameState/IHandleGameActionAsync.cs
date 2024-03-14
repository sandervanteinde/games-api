using GameApis.Shared.Dtos;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState;

public interface IHandleGameActionAsync<TGameContext, TAction>
    where TGameContext : IGameContext<TGameContext>
    where TAction : IAction
{
    Task<OneOf<Success, ActionFailed>> HandleActionAsync(ActionContext<TGameContext, TAction> actionContext);
}