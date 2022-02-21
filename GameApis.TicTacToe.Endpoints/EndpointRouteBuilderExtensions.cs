using GameApis.Shared.GameState;
using GameApis.Shared.Services;
using GameApis.TicTacToe.GameState;
using GameApis.TicTacToe.GameState.States;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace GameApis.TicTacToe.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder UseTicTacToeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tic-tac-toe", async ([FromServicesAttribute] IGameRepository<TicTacToeContext> gameRepository) =>
        {
            var gameEngine = GameEngine.Create<WaitForPlayersContext, TicTacToeContext>();
            var gameId = await gameRepository.PersistGameEngineAsync(gameEngine);
            return gameId;
        });
        return app;
    }
}
