using GameApis.Shared.GameState;
using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Services;

public interface IGameRepository<TGameContext>
{
    Task<OneOf<GameEngine<TGameContext>, NotFound>> GetGameEngineAsync(GameId gameId);
    Task<GameId> PersistGameEngineAsync(GameEngine<TGameContext> gameEngine);
}