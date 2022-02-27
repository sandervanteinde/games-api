using AntDesign;
using GameApis.Examples.Client;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace GameApis.Examples.Pages;

public partial class HostOrJoinGame
{
    private readonly Form _form = new();
    private Form<Form>? _antForm;
    private bool _isVerifyingGame;

    [Parameter] public string? GameIdentifier { get; set; }

    [Inject] public GameApiClient? GameApiClient { get; set; }
    [Inject] public NavigationManager? NavigationManager { get; set; }

    private async Task OnFinish()
    {
        Console.WriteLine("Finish");
        if (_antForm?.Validate() is true)
        {
            await JoinGame();
        }
    }

    private async Task StartHostGame()
    {
        _isVerifyingGame = true;
        try
        {
            ArgumentNullException.ThrowIfNull(GameApiClient);
            var gameId = await CreateGameForIdentifier();
            NavigateToGame(gameId);
        }
        finally
        {
            _isVerifyingGame = false;
            StateHasChanged();
        }
    }

    private async Task JoinGame()
    {
        _isVerifyingGame = true;
        try
        {
            ArgumentNullException.ThrowIfNull(GameApiClient);
            var game = await GetGameForIdentifier(_form.GameId!);
            if (game is not null)
            {
                NavigateToGame(Guid.Parse(_form.GameId!));
            }
        }
        finally
        {
            _isVerifyingGame = false;
            StateHasChanged();
        }
    }

    private void NavigateToGame(Guid gameId)
    {
        ArgumentNullException.ThrowIfNull(NavigationManager);
        NavigationManager.NavigateTo($"/{GameIdentifier}/{gameId}");
    }

    private Task<Guid> CreateGameForIdentifier()
    {
        ArgumentNullException.ThrowIfNull(GameApiClient);
        return GameIdentifier switch
        {
            "tic-tac-toe" => GameApiClient.CreateTicTacToeGameAsync(),
            _ => throw new InvalidOperationException("Unknown game id")
        };
    }

    private async Task<object?> GetGameForIdentifier(string gameId)
    {
        ArgumentNullException.ThrowIfNull(GameApiClient);
        return GameIdentifier switch
        {
            "tic-tac-toe" => await GameApiClient.GetTicTacToeGameByIdAsync(gameId),
            _ => null
        };
    }

    public class Form
    {
        [Required]
        [RegularExpression(@"(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$",
            ErrorMessage = "Invalid game id")]
        public string? GameId { get; set; }
    }
}