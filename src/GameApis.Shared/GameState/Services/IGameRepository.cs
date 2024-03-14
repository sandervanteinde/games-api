using OneOf;
using OneOf.Types;

namespace GameApis.Shared.GameState.Services;

public interface IGameRepository<TGameContext>
    where TGameContext : IGameContext<TGameContext>
{
    Task<OneOf<GameEngine<TGameContext>, NotFound>> GetGameEngineAsync(GameId gameId);
    Task PersistGameEngineAsync(GameId gameId, GameEngine<TGameContext> gameEngine);
}