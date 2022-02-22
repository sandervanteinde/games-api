using GameApis.Shared.GameState;
using GameApis.Shared.GameState.Services;
using System.Reflection;

namespace GameApis.WebHost;

/// <summary>
/// Utility for making typed endpoint builders for genericly set up games
/// </summary>
internal static class EndpointHandlers
{
    public static Delegate HandleCreateGame(Type gameContextType, Type initialStateType)
    {
        return CreateDelegateForMethod(nameof(HandleCreateGameGeneric), gameContextType, initialStateType);
    }

    public static Delegate HandleGetGame(Type gameContextType)
    {
        return CreateDelegateForMethod(nameof(HandleGetGameGeneric), gameContextType);
    }

    public static Delegate HandleActionEndpoint(Type gameContextType, Type actionType)
    {
        return CreateDelegateForMethod(nameof(HandleActionEndpointGeneric), gameContextType, actionType);
    }

    private static Delegate HandleActionEndpointGeneric<TGameContext, TAction>()
        where TGameContext : IGameContext
        where TAction : IAction
    {
        return async (TAction action, Guid gameId, IGameActionHandler<TGameContext> actionHandler) =>
        {
            var actionResult = await actionHandler.HandleGameActionAsync(new(gameId), action);
            return actionResult.Match(
                success => Results.NoContent(),
                actionFailed => Results.BadRequest(actionFailed)
            );
        };
    }

    private static Delegate HandleCreateGameGeneric<TGameContext, TInitialState>()
        where TGameContext : IGameContext, new()
        where TInitialState : IGameState<TGameContext>
    {
        return async (IGameRepository<TGameContext> gameRepository, TInitialState initialState) =>
        {
            var gameEngine = new GameEngine<TGameContext>(initialState, new TGameContext());
            var gameId = await gameRepository.PersistGameEngineAsync(gameEngine);
            return gameId.Value;
        };
    }

    private static Delegate HandleGetGameGeneric<TGameContext>()
        where TGameContext : IGameContext
    {
        return async (Guid gameId, IGameRepository<TGameContext> gameRepository) =>
        {
            var gameEngineResult = await gameRepository.GetGameEngineAsync(new(gameId));
            return gameEngineResult.Match(
                gameEngine => Results.Ok(gameEngine.GameContext),
                notFound => Results.NotFound()
            );
        };
    }

    private static Delegate CreateDelegateForMethod(string methodName, params Type[] genericArguments)
    {
        var method = typeof(EndpointHandlers).GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException($"Did not find {nameof(methodName)} method");
        var genericMethod = method.MakeGenericMethod(genericArguments);
        return (Delegate)genericMethod.Invoke(null, Array.Empty<object>())!;
    }
}
