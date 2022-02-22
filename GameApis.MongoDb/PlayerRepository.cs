using GameApis.Shared.Players;
using GameApis.Shared.Players.Services;
using MongoDB.Driver;
using OneOf;
using OneOf.Types;

namespace GameApis.MongoDb;

internal class PlayerRepository : IPlayerRepository
{
    private readonly IMongoClient mongoClient;

    public PlayerRepository(IMongoClient mongoClient)
    {
        this.mongoClient = mongoClient;
    }
    public async Task<OneOf<Player, NotFound>> GetPlayerByInternalIdAsync(InternalPlayerId internalId)
    {
        var collection = GetCollection();
        var cursor = await collection.FindAsync(player => player.InternalId == internalId.Value);
        var foundPlayer = await cursor.FirstOrDefaultAsync();
        if (foundPlayer is null)
        {
            return new NotFound();
        }

        return new Player(
            new PlayerId(
                new InternalPlayerId(foundPlayer.InternalId),
                new ExternalPlayerId(foundPlayer.ExternalId)
            ),
            foundPlayer.Name
        );
    }

    public Task StorePlayerAsyc(Player player)
    {
        var collection = GetCollection();
        var entry = new PlayerEntry
        {
            ExternalId = player.Id.ExternalId.Value,
            InternalId = player.Id.InternalId.Value,
            Name = player.Name
        };

        return collection.InsertOneAsync(entry);
    }

    private IMongoCollection<PlayerEntry> GetCollection()
    {
        return mongoClient.GetDatabase("GameApi")
            .GetCollection<PlayerEntry>("Players");
    }
}
