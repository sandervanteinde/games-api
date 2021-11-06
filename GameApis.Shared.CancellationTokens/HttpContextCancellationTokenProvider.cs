using Microsoft.AspNetCore.Http;

namespace GameApis.Shared.CancellationTokens;

public class HttpContextCancellationTokenProvider : ICancellationTokenProvider
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CancellationToken CancellationToken => httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

    public HttpContextCancellationTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }
}
