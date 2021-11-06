namespace GameApis.Shared.CancellationTokens;

public class NoneCancellationTokenProvider : ICancellationTokenProvider
{
    public CancellationToken CancellationToken { get; } = CancellationToken.None;
}
