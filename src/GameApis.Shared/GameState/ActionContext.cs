using GameApis.Shared.Players;

namespace GameApis.Shared.GameState;

public record ActionContext<TGameContext, TAction>(PlayerId PlayerPerformingAction, TAction Action, TGameContext Context, GameEngine<TGameContext> Engine)
    where TGameContext : IGameContext<TGameContext>
    where TAction : IAction;