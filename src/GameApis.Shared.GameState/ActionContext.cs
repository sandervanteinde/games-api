namespace GameApis.Shared.GameState;

public record ActionContext<TGameContext, TAction>(TAction Action, TGameContext Context, GameEngine<TGameContext> Engine);
