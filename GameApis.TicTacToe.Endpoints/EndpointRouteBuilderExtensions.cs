using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace GameApis.TicTacToe.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder UseTicTacToeEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/tic-tac-toe", () => "Hello World!");
        return app;
    }
}
