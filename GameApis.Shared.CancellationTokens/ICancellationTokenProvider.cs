namespace GameApis.Shared.CancellationTokens;

public interface ICancellationTokenProvider
{
    CancellationToken CancellationToken { get; }
}
