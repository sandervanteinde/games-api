using GameApis.Shared.Dtos;
using GameApis.Shared.GameState;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Services;

internal interface IGameActionHandler<TGameContext>
{
    Task<OneOf<Success, ActionFailed>> HandleGameActionAsync(GameId gameId, IAction gameAction);
}