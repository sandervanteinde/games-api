using GameApis.Shared.Dtos;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

public interface IGameActionHandler<TGameContext>
{
    Task<OneOf<Success, ActionFailed>> HandleGameActionAsync(GameId gameId, IAction gameAction);
}