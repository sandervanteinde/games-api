using GameApis.Shared.Dtos;
using JetBrains.Annotations;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

[UsedImplicitly]
public interface IGameActionHandler<TGameContext>
    where TGameContext : IGameContext<TGameContext>
{
    Task<OneOf<Success, ActionFailed>> HandleGameActionAsync<TAction>(GameId gameId, TAction gameAction)
        where TAction : IAction;
}