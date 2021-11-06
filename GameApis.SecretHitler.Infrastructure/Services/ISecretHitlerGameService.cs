using GameApis.SecretHitler.Domain.Entities;

namespace GameApis.SecretHitler.Infrastructure.Services;

public interface ISecretHitlerGameService
{
    Task<SecretHitlerGame> CreateNewAsync();
    Task<SecretHitlerGame> UpdateSecretHitlerGameAsync(Guid gameId, Action<SecretHitlerGame> mutator);
    Task<SecretHitlerGame> GetGameAsync(Guid gameId);
}
