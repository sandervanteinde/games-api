using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

public interface IGameRepository<TGameContext>
{
    Task<OneOf<GameEngine<TGameContext>, NotFound>> GetGameEngineAsync(GameId gameId);
    Task<GameId> PersistGameEngineAsync(GameEngine<TGameContext> gameEngine);
}