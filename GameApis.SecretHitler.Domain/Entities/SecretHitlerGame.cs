using GameApis.Domain.Core;
using GameApis.Domain.Exceptions;
using GameApis.SecretHitler.Domain.Events;
using GameApis.SecretHitler.Domain.Extensions;

namespace GameApis.SecretHitler.Domain.Entities;

public partial class SecretHitlerGame : AggregateRoot<Guid>
{
    public const int MIN_PLAYERS = 5;
    public const int MAX_PLAYERS = 10;
    private readonly List<Player> _players = new(10);

    public Deck? DrawPile { get; private set; }
    public Deck? DiscardPile { get; private set; }

    public IReadOnlyList<Player> Players => _players.AsReadOnly();
    public GameState State { get; private set; } = GameState.NotStarted;

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

        if (State is not GameState.NotStarted)
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
        if (State is not GameState.NotStarted)
        {
            throw new DomainException(DomainExceptionCodes.GameHasStarted);
        }

        if (_players.Count < MIN_PLAYERS)
        {
            throw new DomainException(DomainExceptionCodes.NotEnoughPlayers);
        }

        var roles = AvailableRolesForPlayerCount(Players.Count);
        roles.ShuffleRandomly();
        var assignments = roles
            .Select((role, i) => new GameStarted.RoleAssignment
            {
                PlayerId = _players[i].Id,
                Role = role
            })
            .ToArray();

        var drawDeck = GenerateDrawDeck();
        var randomPresident = _players.ToArray().ShuffleRandomly().First();
        var gameStartedEvent = new GameStarted
        {
            GameId = Id,
            Assignments = assignments,
            Cards = drawDeck,
            InitialPresidentId = randomPresident.Id
        };
        RaiseEvent(gameStartedEvent);
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
        State = GameState.Started;
        DrawPile = new Deck(gameStarted.Cards);
        var playersById = _players.ToDictionary(player => player.Id);
        foreach (var assignment in gameStarted.Assignments)
        {
            if (!playersById.TryGetValue(assignment.PlayerId, out var player))
            {
                throw new InvalidOperationException($"No player found with Id {assignment.PlayerId}");
            }

            player.Role = assignment.Role;
        }
    }

    private void HandleEvent(PlayerLeft playerLeft)
    {
        var index = _players.FindIndex(player => player.Id == playerLeft.PlayerId);
        _players.RemoveAt(index);
    }

    public static Card[] GenerateDrawDeck()
    {
        return Enumerable.Repeat(Card.Liberal, 8)
            .Concat(Enumerable.Repeat(Card.Fascist, 11))
            .ToArray()
            .ShuffleRandomly();
    }

    public static Role[] AvailableRolesForPlayerCount(int playerCount)
    {
        return EnumerateRoles().ToArray();
        IEnumerable<Role> EnumerateRoles()
        {
            yield return Role.Hitler;
            yield return Role.Liberal;
            yield return Role.Liberal;
            yield return Role.Liberal;
            yield return Role.Fascist;
            if (playerCount >= 6)
            {
                yield return Role.Liberal;
            }
            if (playerCount >= 7)
            {
                yield return Role.Fascist;
            }
            if (playerCount >= 8)
            {
                yield return Role.Liberal;
            }
            if (playerCount >= 9)
            {
                yield return Role.Fascist;
            }
            if (playerCount == 10)
            {
                yield return Role.Liberal;
            }
        }
    }
}
