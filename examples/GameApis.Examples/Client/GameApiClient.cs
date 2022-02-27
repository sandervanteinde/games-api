using GameApis.Examples.Services;

namespace GameApis.Examples.Client;

#pragma warning disable IDE0060 // Remove unused parameter --generated method
public partial class GameApiClient
{
    private readonly IPlayerIdAccessor _playerIdAccessor;

    public GameApiClient(string baseAddress, HttpClient client, IPlayerIdAccessor playerIdAccessor)
        : this(baseAddress, client)
    {
        _playerIdAccessor = playerIdAccessor;
    }

    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
    {
        var playerId = _playerIdAccessor.GetPlayerId();
        if (playerId is not null)
        {
            request.Headers.Add("X-Player-Id", playerId.InternalId.Value.ToString());
        }
    }
}
#pragma warning restore IDE0060 // Remove unused parameter
