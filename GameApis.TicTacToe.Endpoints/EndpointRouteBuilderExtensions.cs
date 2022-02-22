using Microsoft.AspNetCore.Routing;

namespace GameApis.TicTacToe.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder UseTicTacToeEndpoints(this IEndpointRouteBuilder app)
    {
        //const string EndpointName = "/api/tic-tac-toe";
        //const string SpecificGameEndpoint = $"{EndpointName}/{{gameId}}";
        //app.MapPost(EndpointName, async (IGameRepository<TicTacToeContext> gameRepository) =>
        //{
        //    var gameEngine = GameEngine.Create<WaitForPlayersState, TicTacToeContext>();
        //    var gameId = await gameRepository.PersistGameEngineAsync(gameEngine);
        //    return gameId.Value;
        //});
        //app.MapGet(SpecificGameEndpoint, async ([FromRoute] Guid gameId, IGameRepository<TicTacToeContext> gameRepository) =>
        //{
        //    var gameIdTyped = new GameId(gameId);
        //    var gameEngineResult = await gameRepository.GetGameEngineAsync(gameIdTyped);

        //    return gameEngineResult.Match(
        //        gameEngine => Results.Ok(gameEngine),
        //        notFound => Results.NotFound()
        //    );
        //})
        //    .Produces(404)
        //    .Produces(200, typeof(GameEngine<TicTacToeContext>));
        return app;
    }
}
