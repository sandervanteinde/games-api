using GameApis.SecretHitler.Domain.Entities;
using GameApis.Shared.MongoAggregateStorage.Services;

namespace GameApis.SecretHitler.Infrastructure.Services;

internal class SecretHitlerGameService : ISecretHitlerGameService
{
    private readonly IAggregateStorage<SecretHitlerGame, Guid> storage;

    public SecretHitlerGameService(IAggregateStorage<SecretHitlerGame, Guid> storage)
    {
        this.storage = storage;
    }

    public async Task<SecretHitlerGame> CreateNewAsync()
    {
        var game = SecretHitlerGame.CreateNew();
        await storage.SaveEventsAsync(game);
        return game;
    }

    public async Task<SecretHitlerGame> GetGameAsync(Guid gameId)
    {
        var events = await storage.GetDomainEventsAsync(gameId);
        var game = new SecretHitlerGame(gameId, events);
        return game;
    }

    public async Task<SecretHitlerGame> UpdateSecretHitlerGameAsync(Guid gameId, Action<SecretHitlerGame> mutator)
    {
        var events = await storage.GetDomainEventsAsync(gameId);
        var game = new SecretHitlerGame(gameId, events);
        mutator(game);
        await storage.SaveEventsAsync(game);
        return game;
    }
}
