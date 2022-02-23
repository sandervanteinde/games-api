using GameApis.Examples.TicTacToe.Services;

namespace GameApis.Examples.TicTacToe.Client;

#pragma warning disable IDE0060 // Remove unused parameter --generated method
public partial class TicTacToeClient
{
    private readonly IPlayerIdAccessor playerIdAccessor;

    public TicTacToeClient(string baseAddress, HttpClient client, IPlayerIdAccessor playerIdAccessor)
        : this(baseAddress, client)
    {
        this.playerIdAccessor = playerIdAccessor;
    }

    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
    {
        if (playerIdAccessor.TryGetPlayerId(out var playerId))
        {
            request.Headers.Add("X-Player-Id", playerId.InternalId.Value.ToString());
        }
    }
}
#pragma warning restore IDE0060 // Remove unused parameter
