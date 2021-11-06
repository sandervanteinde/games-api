using GameApis.Domain.Core;
using GameApis.Domain.Exceptions;
using GameApis.SecretHitler.Domain.Events;

namespace GameApis.SecretHitler.Domain.Entities;

public partial class SecretHitlerGame : AggregateRoot<Guid>
{
    public const int MIN_PLAYERS = 5;
    public const int MAX_PLAYERS = 10;
    private readonly List<Player> _players = new(10);
    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    public bool HasGameStarted { get; private set; }

    private SecretHitlerGame(Guid id) : base(id) { }
    public SecretHitlerGame(Guid id, IEnumerable<DomainEvent> events) : base(id, events) { }

    public static SecretHitlerGame CreateNew()
    {
        var newId = Guid.NewGuid();
        var game = new SecretHitlerGame(newId);
        var newGameEvent = new GameCreated { GameId = newId };
        game.RaiseEvent(newGameEvent);
        return game;
    }

    public void AddPlayer(string name)
    {
        if (_players.Count is MAX_PLAYERS)
        {
            throw new DomainException(DomainExceptionCodes.LobbyFull);
        }

        if (HasGameStarted)
        {
            throw new DomainException(DomainExceptionCodes.GameHasStarted);
        }

        if (_players.Any(player => player.Name == name))
        {
            throw new DomainException(DomainExceptionCodes.DuplicatePlayerName);
        }

        var newPlayerId = Guid.NewGuid();
        RaiseEvent(new PlayerJoined { GameId = Id, PlayerId = newPlayerId, PlayerName = name });
    }

    public void PlayerLeaves(Guid playerId)
    {
        if (_players.All(player => player.Id != playerId))
        {
            throw new DomainException(DomainExceptionCodes.PlayerNotFound);
        }

        var playerLeft = new PlayerLeft { GameId = Id, PlayerId = playerId };
        RaiseEvent(playerLeft);
    }

    public void StartGame()
    {
        if (HasGameStarted)
        {
            throw new DomainException(DomainExceptionCodes.GameHasStarted);
        }

        if (_players.Count < MIN_PLAYERS)
        {
            throw new DomainException(DomainExceptionCodes.NotEnoughPlayers);
        }

        RaiseEvent(new GameStarted { GameId = Id });
    }

    public override string GetUniqueIdentifier()
    {
        return Id.ToString();
    }

    protected override void When(dynamic @event)
    {
        HandleEvent(@event);
    }

    private void HandleEvent(PlayerJoined playerJoined)
    {
        var player = new Player(playerJoined.PlayerId, playerJoined.PlayerName);
        _players.Add(player);
    }

    private void HandleEvent(GameCreated gameCreated)
    {
        _players.Clear();
    }

    private void HandleEvent(GameStarted gameStarted)
    {
        HasGameStarted = true;
    }

    private void HandleEvent(PlayerLeft playerLeft)
    {
        var index = _players.FindIndex(player => player.Id == playerLeft.PlayerId);
        _players.RemoveAt(index);
    }
}
