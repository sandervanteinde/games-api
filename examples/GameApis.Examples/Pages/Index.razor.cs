using GameApis.Examples.Models;
using GameApis.Examples.Services;
using Microsoft.AspNetCore.Components;

namespace GameApis.Examples.Pages;

public partial class Index
{
    private IEnumerable<GameEntry> _gameEntries = Array.Empty<GameEntry>();
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Inject] public IGameRegistry GameRegistry { get; set; } = null!;

    protected override void OnInitialized()
    {
        _gameEntries = GameRegistry.GetAllGames();
    }

    private void SelectGame(GameEntry game)
    {
        Navigation.NavigateTo($"/{game.Identifier}");
    }
}