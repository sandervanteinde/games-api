using GameApis.Shared.Dtos;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

public interface IGameActionHandler<TGameContext>
    where TGameContext : IGameContext
{
    Task<OneOf<Success, ActionFailed>> HandleGameActionAsync<TAction>(GameId gameId, TAction gameAction)
        where TAction : IAction;
}