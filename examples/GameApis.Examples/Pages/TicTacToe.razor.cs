using GameApis.Examples.Client;
using Microsoft.AspNetCore.Components;

namespace GameApis.Examples.Pages;

public partial class TicTacToe
{
    private TicTacToeContext? _context;
    private State _state = State.Loading;

    [Inject] public GameApiClient? GameApiClient { get; set; }
    [CascadingParameter] public PlayerId? PlayerId { get; set; }

    [Parameter] public Guid GameId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ArgumentNullException.ThrowIfNull(GameApiClient);
        ArgumentNullException.ThrowIfNull(PlayerId);

        var result = await GameApiClient.GetTicTacToeGameByIdAsync(GameId.ToString());
        _context = result.Game;
        _state = State.Joining;
        if (_context.PlayerUsingO.Value == PlayerId.ExternalId.Value)
        {
            _state = State.WaitingInLobby;
            return;
        }

        if (_context.PlayerUsingX.Value == PlayerId.ExternalId.Value)
        {
            _state = State.WaitingInLobby;
            return;
        }

        if (_context is {PlayerUsingO: not null, PlayerUsingX: not null})
        {
            _state = State.LobbyFull;
            return;
        }

        await GameApiClient.PerformJoinPlayerActionOnTicTacToeGameAsync(GameId.ToString(), new JoinPlayerAction());
        _state = State.WaitingInLobby;
    }

    private enum State
    {
        Loading,
        Joining,
        LobbyFull,
        WaitingInLobby
    }
}