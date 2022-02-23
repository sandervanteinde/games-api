using GameApis.Shared;
using GameApis.Shared.GameState;
using GameApis.Shared.GameState.Services;
using MongoDB.Driver;
using OneOf;
using OneOf.Types;

namespace GameApis.MongoDb;

internal class MongoGameRepository<TGameContext> : IGameRepository<TGameContext>
    where TGameContext : IGameContext
{
    private readonly IMongoClient mongoClient;
    private readonly IGameStateResolver<TGameContext> gameStateResolver;

    public MongoGameRepository(
        IMongoClient mongoClient,
        IGameStateResolver<TGameContext> gameStateResolver
    )
    {
        this.mongoClient = mongoClient;
        this.gameStateResolver = gameStateResolver;
    }

    public async Task<OneOf<GameEngine<TGameContext>, NotFound>> GetGameEngineAsync(GameId gameId)
    {
        var collection = GetCollection();
        var entryCursor = await collection.FindAsync(entry => entry.Id == gameId.Value);
        var entry = await entryCursor.FirstOrDefaultAsync();

        if (entry is null)
        {
            return new NotFound();
        }

        var gameStateResult = gameStateResolver.ResolveGameState(entry.CurrentState);
        if (gameStateResult.TryPickT1(out _, out var gameState))
        {
            throw new InvalidOperationException($"Attempted to resolve game state {entry.CurrentState} for game context {typeof(TGameContext).Name} but it did not exist.");
        }

        return new GameEngine<TGameContext>(gameState, entry.GameContext);
    }

    public Task PersistGameEngineAsync(GameId gameId, GameEngine<TGameContext> gameEngine)
    {
        var collection = GetCollection();
        return collection.ReplaceOneAsync(entry => entry.Id == gameId.Value, new MongoEntry<TGameContext>
        {
            Id = gameId.Value,
            GameContext = gameEngine.GameContext,
            CurrentState = gameEngine.GameState.GetType().Name
        }, new ReplaceOptions { IsUpsert = true });
    }

    private IMongoCollection<MongoEntry<TGameContext>> GetCollection()
    {
        return mongoClient.GetDatabase("GameApi")
            .GetCollection<MongoEntry<TGameContext>>(typeof(TGameContext).Name);
    }
}
