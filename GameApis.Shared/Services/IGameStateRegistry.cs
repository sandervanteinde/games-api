using OneOf;
using OneOf.Types;

namespace GameApis.Shared.Services;

internal interface IGameStateRegistry
{
    OneOf<Type, NotFound> GetGameStateForGameContext<GameContext>(string name);
    void RegisterGameState(Type gameContext, Type gameState);
}