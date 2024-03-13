using GameApis.Shared.Dtos;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState;

public interface IHandleGameAction<TGameContext, TAction>
    where TGameContext : IGameContext
    where TAction : IAction
{
    OneOf<Success, ActionFailed> HandleAction(ActionContext<TGameContext, TAction> actionContext);
}