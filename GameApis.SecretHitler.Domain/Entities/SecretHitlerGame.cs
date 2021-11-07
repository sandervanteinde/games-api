using GameApis.Domain.Core;
using GameApis.Domain.Exceptions;
using GameApis.SecretHitler.Domain.Events;
using GameApis.SecretHitler.Domain.Extensions;
using System.Runtime.CompilerServices;

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
    public int AmountOfFailedVotes { get; private set; }

    public Player? ElectedPresident { get; private set; }
    public Player? ElectedChancellor { get;private set; }

    public Player? President { get; private set; }
    public Player? Chancellor { get; private set; }

    public Player? PreviousPresident { get; private set; }
    public Player? PreviousChancellor { get; private set; }

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
        var newInternalPlayerId = Guid.NewGuid();
        var playerJoined = new PlayerJoined
        {
            GameId = Id,
            ExternalPlayerId = newPlayerId,
            InternalPlayerId = newInternalPlayerId,
            PlayerName = name
        };
        RaiseEvent(playerJoined);
    }

    public void PlayerLeaves(Guid playerId)
    {
        if (_players.All(player => player.Id.InternalId != playerId))
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
                PlayerId = _players[i].Id.InternalId,
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
            InitialPresidentId = randomPresident.Id.InternalId
        };
        RaiseEvent(gameStartedEvent);
    }

    public void ElectChancellor(Guid playerId, Guid playerExternalId)
    {
        if(playerId != ElectedPresident?.Id.InternalId)
        {
            throw new DomainException(DomainExceptionCodes.PlayerCantPerformAction);
        }
        var electedPlayer = _players.Find(player => player.Id.ExternalId == playerExternalId);
        if(electedPlayer is null)
        {
            throw new DomainException(DomainExceptionCodes.InvalidTargetPlayer);
        }

        var alivePlayers = _players.Where(player => player.Alive).Count();
        var canPreviousPresidentBeElected = alivePlayers <= 5;

        if(
            (electedPlayer == PreviousPresident && !canPreviousPresidentBeElected) 
            || electedPlayer == PreviousChancellor
            || electedPlayer == ElectedPresident)
        {
            throw new DomainException(DomainExceptionCodes.InvalidTargetPlayer);
        }

        var playerElected = new ChancellorElected {  GameId = Id, ChancellorId = electedPlayer.Id.InternalId };
        RaiseEvent(playerElected);
    }

    public void CastVote(Guid playerId, Vote vote)
    {
        var player = _players.Find(player => player.Id.InternalId == playerId);
        if (player is null)
        {
            throw new InvalidOperationException();
        }

        if (player.CastVote is not null)
        {
            throw new DomainException(DomainExceptionCodes.ActionAlreadyPerformed);
        }

        RaiseEvent(new VoteCast
        {
            GameId = Id,
            PlayerId = playerId,
            Vote = vote
        });

        if(_players.Any(player => player.CastVote is null))
        {
            return;
        }

        var votesCast = new VoteResults
        {
            GameId = Id,
            Results = _players
                .Select(player => new VoteResults.VoteResult
                {
                    ExternalPlayerId = player.Id.ExternalId,
                    Vote = player.CastVote ?? throw new InvalidOperationException()
                })
                .ToArray()
        };
        RaiseEvent(votesCast);
    }

    protected override void When(dynamic @event)
    {
        HandleEvent(@event);
    }

    private void HandleEvent(VoteSucceeded voteSucceeded)
    {
        President = ElectedPresident;
        Chancellor = ElectedChancellor;
        ElectedChancellor = null;
        ElectedPresident = null;
        AmountOfFailedVotes = 0;

        State = GameState.PresidentPicksCardToThrow;
        // TODO: Give cards to president for picking.
    }

    private void HandleEvent(VoteFailed voteFailed)
    {
        var presidentIndex = _players.IndexOf(ElectedPresident!);
        Player nextPresident;
        do
        {
            nextPresident = _players[(presidentIndex + 1) % _players.Count];
            presidentIndex++;
        } while (nextPresident.Alive is false);
        ElectedPresident = nextPresident;
        ElectedChancellor = null;
        State = GameState.PickAChancellor;
        ++AmountOfFailedVotes;
    }

    private void HandleEvent(VoteResults results)
    {
        var votes = results.Results.ToLookup(player => player.Vote == Vote.Yes);
        var votesPassed = votes[true].Count() > votes[false].Count();
        _players.ForEach(player => player.CastVote = null);
        RaiseEvent(votesPassed ? new VoteSucceeded() : new VoteFailed());
    }

    private void HandleEvent(VoteCast voteCast)
    {
        var player = _players.Find(player => player.Id.InternalId == voteCast.PlayerId);
        if(player is null)
        {
            throw new InvalidOperationException();
        }

        player.CastVote = voteCast.Vote;
    }

    private void HandleEvent(ChancellorElected chancellorElected)
    {
        var player = _players.Find(player => player.Id.InternalId == chancellorElected.ChancellorId);
        if(player is null)
        {
            throw new InvalidOperationException();
        }
        ElectedChancellor = player;
        State = GameState.Voting;
    }

    private void HandleEvent(PlayerJoined playerJoined)
    {
        var player = new Player(playerJoined.InternalPlayerId, playerJoined.ExternalPlayerId, playerJoined.PlayerName);
        _players.Add(player);
    }

    private void HandleEvent(GameCreated gameCreated)
    {
        _players.Clear();
    }

    private void HandleEvent(GameStarted gameStarted)
    {
        State = GameState.PickAChancellor;
        DrawPile = new Deck(gameStarted.Cards);
        var playersById = _players.ToDictionary(player => player.Id.InternalId);
        foreach (var assignment in gameStarted.Assignments)
        {
            if (!playersById.TryGetValue(assignment.PlayerId, out var player))
            {
                throw new InvalidOperationException($"No player found with Id {assignment.PlayerId}");
            }

            player.Role = assignment.Role;
        }
        ElectedPresident = playersById[gameStarted.InitialPresidentId];
    }

    private void HandleEvent(PlayerLeft playerLeft)
    {
        var index = _players.FindIndex(player => player.Id.InternalId == playerLeft.PlayerId);
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
