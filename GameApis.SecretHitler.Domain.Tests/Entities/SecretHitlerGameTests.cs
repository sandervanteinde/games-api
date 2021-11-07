using FluentAssertions;
using GameApis.Domain.Exceptions;
using GameApis.SecretHitler.Domain.Entities;
using GameApis.SecretHitler.Domain.Events;
using GameApis.SecretHitler.Domain.Tests.Extensions;
using System;
using System.Linq;
using Xunit;

namespace GameApis.SecretHitler.Domain.Tests.Entities;

public class SecretHitlerGameTests
{
    private readonly SecretHitlerGame _sut;

    public SecretHitlerGameTests()
    {
        _sut = SecretHitlerGame.CreateNew();
    }

    [Fact]
    public void CreateNew_ResultsInGameCreatedEvent()
    {
        // arrange in ctor
        // act in ctor
        var newEvent = _sut.GetEvents().First();

        // assert
        newEvent.Should().BeOfType<GameCreated>().Which.GameId.Should().Be(_sut.Id);
    }

    [Fact]
    public void AddPlayer_EmitsEvent_WhenSuccessfull()
    {
        // arrange
        const string playerName = "Sander";

        // act
        _sut.AddPlayer(playerName);

        // assert
        _sut.GetEvents().Last().Should().BeOfType<PlayerJoined>().Which.PlayerName.Should().Be(playerName);
    }

    [Fact]
    public void AddPlayer_WhenPlayerWithNameExists_ThrowsException()
    {
        // arrange
        const string playerName = "Sander";
        _sut.AddPlayer(playerName);

        // act
        var act = () => _sut.AddPlayer(playerName);

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.DuplicatePlayerName);
    }

    [Fact]
    public void AddPlayer_ThrowsDomainException_WhenTenPeopleAreInTheGame()
    {
        // arrange
        for (var i = 0; i < SecretHitlerGame.MAX_PLAYERS; i++)
        {
            _sut.AddPlayer($"Player {i}");
        }

        // act
        var act = () => _sut.AddPlayer("Player 11");

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.LobbyFull);
    }

    [Fact]
    public void AddPlayer_ThrowsDomainException_WhenGameHasStarted()
    {
        // arrange
        SetGameInWaitingWithMinimumPlayers();
        _sut.StartGame();

        // act
        var act = () => _sut.AddPlayer("Player 11");

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.GameHasStarted);
    }

    [Fact]
    public void AddPlayer_AddPlayerToPlayerList()
    {
        // arrange
        const string playerName = "Sander";

        // act
        _sut.AddPlayer(playerName);

        // assert
        _sut.Players.Should().ContainEquivalentOf(new { Name = playerName });
    }

    [Fact]
    public void PlayerLeaves_ThrowsException_WhenPlayerDoesNotExist()
    {
        // arrange
        var randomPlayerId = Guid.NewGuid();

        // act
        var act = () => _sut.PlayerLeaves(randomPlayerId);

        // arrange
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.PlayerNotFound);
    }

    [Fact]
    public void PlayerLeaves_WhenPlayerExists_ResultsInLeaveEvent()
    {
        // arrange
        _sut.AddPlayer("Bogus");
        var playerId = _sut.Players.First().Id.InternalId;

        // act
        _sut.PlayerLeaves(playerId);

        // assert
        var leaveEvent = _sut.GetEvents().Last();
        leaveEvent.Should().BeOfType<PlayerLeft>().Which.PlayerId.Should().Be(playerId);
    }

    [Fact]
    public void PlayerLeaves_WhenPlayerExists_RemovesPlayer()
    {
        // arrange
        _sut.AddPlayer("Bogus");
        var playerId = _sut.Players.First().Id.InternalId;

        // act
        _sut.PlayerLeaves(playerId);

        // assert
        _sut.Players.Should().BeEmpty();
    }

    [Fact]
    public void StartGame_WithEnoughPlayers_ResultsInStartGameEvent()
    {
        // arrange
        SetGameInWaitingWithMinimumPlayers();

        // act
        _sut.StartGame();

        // assert
        var gameStartedEvent = _sut.GetEvents().OfType<GameStarted>().First();
        gameStartedEvent.GameId.Should().Be(_sut.Id);
    }

    [Fact]
    public void StartGame_WithGameAlreadyStarted_ResultsInDomainException()
    {
        // arrange
        SetGameInWaitingWithMinimumPlayers();
        _sut.StartGame();

        // act
        var act = () => _sut.StartGame();

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.GameHasStarted);
    }

    [Fact]
    public void StartGame_WithoutEnoughPlayers_ResultsInDomainException()
    {
        // arrange
        // act
        var act = () => _sut.StartGame();

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.NotEnoughPlayers);
    }

    [Fact]
    public void StartGame_WithEnoughPlayers_ChangesStateToStarted()
    {
        // arrange
        SetGameInWaitingWithMinimumPlayers();

        // act
        _sut.StartGame();

        // assert
        _sut.State.Should().Be(GameState.PickAChancellor);
    }

    [Fact]
    public void StartGame_WithEnoughPlayers_AssignsPlayerRoles()
    {
        // arrange
        SetGameInWaitingWithMinimumPlayers();

        // act
        _sut.StartGame();

        // assert
        foreach (var player in _sut.Players)
        {
            player.Role.Should().NotBe(Role.Unassigned);
        }

    }

    [Fact]
    public void RestoringGameThroughCtor_ResultsInSameGame()
    {
        // arrange
        for (var i = 0; i < SecretHitlerGame.MIN_PLAYERS; i++)
        {
            _sut.AddPlayer($"Player {i}");
        }
        _sut.StartGame();
        var events = _sut.GetEvents();

        // act
        var duplicateGame = new SecretHitlerGame(_sut.Id, events);

        // assert
        duplicateGame.Should().BeEquivalentTo(_sut, cfg => cfg.Excluding(e => e.OriginalVersion).ExcludingFields());
    }

    [Theory]
    [InlineData(5, 3, 1)]
    [InlineData(6, 4, 1)]
    [InlineData(7, 4, 2)]
    [InlineData(8, 5, 2)]
    [InlineData(9, 5, 3)]
    [InlineData(10, 6, 3)]
    public void AvailableRolesForPlayerCount_ShouldAlwaysHaveMatchingAmountOfLiberalsAndFascists(int playerCount, int expectedLiberals, int expectedFascists)
    {
        // arrange
        // act
        var roles = SecretHitlerGame.AvailableRolesForPlayerCount(playerCount);
        var amountOfLiberals = roles.Where(role => role is Role.Liberal).Count();
        var amountOfFascists = roles.Where(role => role is Role.Fascist).Count();
        var hasHitler = roles.Any(role => role is Role.Hitler);

        // assert
        hasHitler.Should().BeTrue();
        amountOfFascists.Should().Be(expectedFascists);
        amountOfLiberals.Should().Be(expectedLiberals);
    }

    [Fact]
    public void GenerateDrawDeck_ShouldAlwaysReturnCorrectAmountOfCards()
    {
        // arrange
        // act
        var cards = SecretHitlerGame.GenerateDrawDeck();
        var liberalCards = cards.Where(card => card is Card.Liberal).Count();
        var fascistCards = cards.Where(card => card is Card.Fascist).Count();

        // assert
        liberalCards.Should().Be(8);
        fascistCards.Should().Be(11);
    }

    [Fact]
    public void ElectChancellor_WithValidCandidate_ElectsChancellor()
    {
        // arrange
        SetGameInPickChancellorState();
        var playerThatIsNotPresident = _sut.Players
            .Where(player => player != _sut.ElectedPresident)
            .First();

        // act
        _sut.ElectChancellor(_sut.ElectedPresident!.Id.InternalId, playerThatIsNotPresident.Id.ExternalId);

        // assert
        _sut.ElectedChancellor.Should().Be(playerThatIsNotPresident);
    }

    [Fact]
    public void ElectChancellor_PresidentElectingHimself_ThrowsDomainException()
    {
        // arrange
        SetGameInPickChancellorState();

        // act
        var act = () => _sut.ElectChancellor(_sut.ElectedPresident!.Id.InternalId, _sut.ElectedPresident.Id.ExternalId);

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.InvalidTargetPlayer);
    }

    [Fact]
    public void ElectChancellor_NotElectedPresidentPickingChancellor_ThrowsDomainException()
    {
        // arrange
        SetGameInPickChancellorState();
        var notElectedPresident = _sut.Players
            .First(player => player != _sut.ElectedPresident);
        var validChancellorTarget = _sut.Players
            .First(player => player != notElectedPresident && player != _sut.ElectedPresident);

        // act
        var act = () => _sut.ElectChancellor(notElectedPresident.Id.InternalId, validChancellorTarget.Id.ExternalId);

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.PlayerCantPerformAction);
    }

    [Fact]
    public void ElectChancellor_UnknownPlayerName_ThrowsDomainException()
    {
        // arrange
        SetGameInPickChancellorState();
        Guid guidThatIsNotInternalPlayerId;
        do
        {
            guidThatIsNotInternalPlayerId = Guid.NewGuid();
        }
        while(_sut.Players.Any(player => player.Id.InternalId == guidThatIsNotInternalPlayerId));

        // act
        var act = () => _sut.ElectChancellor(_sut.ElectedPresident!.Id.InternalId, Guid.NewGuid());

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.InvalidTargetPlayer);
    }

    [Fact]
    public void CastVote_EverybodyVotingNo_SetsVotingNoState()
    {
        // arrange
        SetGameInVotingState();
        var initialPresident = _sut.ElectedPresident;

        // act
        foreach(var player in _sut.Players)
        {
            _sut.CastVote(player.Id.InternalId, Vote.No);
        }
        
        // assert
        _sut.ElectedPresident.Should().NotBe(initialPresident);
        _sut.ElectedChancellor.Should().BeNull();
        _sut.State.Should().Be(GameState.PickAChancellor);
        _sut.AmountOfFailedVotes.Should().Be(1);
    }

    [Fact]
    public void CastVote_PlayerVotingTwice_ThrowsDomainException()
    {
        // arrange
        SetGameInVotingState();
        var votingPlayer = _sut.Players[0];
        _sut.CastVote(votingPlayer.Id.InternalId, Vote.Yes);

        // act
        var act = () => _sut.CastVote(votingPlayer.Id.InternalId, Vote.No);

        // assert
        act.Should().ThrowExactly<DomainException>().WithCode(DomainExceptionCodes.ActionAlreadyPerformed);
    }

    private void SetGameInWaitingWithMinimumPlayers()
    {
        for (var i = 0; i < SecretHitlerGame.MIN_PLAYERS; i++)
        {
            _sut.AddPlayer($"Player {i}");
        }
    }

    private void SetGameInPickChancellorState()
    {
        SetGameInWaitingWithMinimumPlayers();
        _sut.StartGame();
    }

    private void SetGameInVotingState()
    {
        SetGameInPickChancellorState();
        var chancellor = _sut.Players.First(player => player != _sut.ElectedPresident);
        _sut.ElectChancellor(_sut.ElectedPresident!.Id.InternalId, chancellor.Id.ExternalId);
    }
}
