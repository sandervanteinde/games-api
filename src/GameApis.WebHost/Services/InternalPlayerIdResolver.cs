using GameApis.Shared.Players;
using GameApis.Shared.Players.Services;
using OneOf;
using OneOf.Types;

namespace GameApis.WebHost.Services;

public class InternalPlayerIdResolver : IInternalPlayerIdResolver
{
    public const string PLAYER_ID_HEADER = "X-Player-Id";
    private readonly IHttpContextAccessor httpContextAccessor;

    private readonly Lazy<OneOf<InternalPlayerId, NotFound>> _playerId;

    public InternalPlayerIdResolver(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        _playerId = new Lazy<OneOf<InternalPlayerId, NotFound>>(ResolveInternalPlayerId);
    }

    public Task<OneOf<InternalPlayerId, NotFound>> ResolveInternalPlayerIdAsync()
    {
        return Task.FromResult(_playerId.Value);
    }

    private OneOf<InternalPlayerId, NotFound> ResolveInternalPlayerId()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return new NotFound();
        }

        if (!httpContext.Request.Headers.TryGetValue(PLAYER_ID_HEADER, out var playerIdAsString))
        {
            return new NotFound();
        }

        if (!Guid.TryParse(playerIdAsString, out var playerId))
        {
            return new NotFound();
        }

        return new InternalPlayerId(playerId);
    }
}