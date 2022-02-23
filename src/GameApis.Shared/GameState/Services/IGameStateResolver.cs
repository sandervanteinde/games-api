using GameApis.Shared.GameState;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

public interface IGameStateResolver<TGameContext>
{
    OneOf<IGameState<TGameContext>, NotFound> ResolveGameState(string gameStateName);
}