using GameApis.Shared;
using GameApis.Shared.GameState;
using GameApis.Shared.Services;
using MongoDB.Driver;
using OneOf;
using OneOf.Types;

namespace GameApis.MongoDb;

internal class MongoGameRepository<TGameContext> : IGameRepository<TGameContext>
{
    private readonly IMongoClient mongoClient;

    public MongoGameRepository(IMongoClient mongoClient)
    {
        this.mongoClient = mongoClient;
    }

    public async Task<OneOf<GameEngine<TGameContext>, NotFound>> GetGameEngineAsync(GameId gameId)
    {
        var collection = GetCollection();

        return new NotFound();
    }

    public async Task<GameId> PersistGameEngineAsync(GameEngine<TGameContext> gameEngine)
    {
        var collection = GetCollection();
        var gameId = GameId.New();
        await collection.InsertOneAsync(new MongoEntry<TGameContext>
        {
            Id = gameId.Value,
            GameContext = gameEngine.GameContext,
            CurrentState = gameEngine.GameState.GetType().Name
        });

        return gameId;
    }

    private IMongoCollection<MongoEntry<TGameContext>> GetCollection()
    {
        return mongoClient.GetDatabase("GameApi")
            .GetCollection<MongoEntry<TGameContext>>(typeof(TGameContext).Name);
    }
}
