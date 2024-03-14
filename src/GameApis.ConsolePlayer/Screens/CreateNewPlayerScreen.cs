using System.Text.RegularExpressions;

namespace GameApis.ConsolePlayer.Screens;

public partial class CreateNewPlayerScreen : IAsyncScreen
{
    [GeneratedRegex("^[a-zA-Z 0-9]+$")]
    private static partial Regex AlphaNumericRegex();

    public async Task RenderAsync()
    {
        var playerName = AnsiConsole.Prompt(
            new TextPrompt<string>("Please provide a player name")
                .Validate(
                    input => AlphaNumericRegex().IsMatch(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]You may only use alphanumeric characters and spaces in your name")
                )
        );
        var playerId = await Api.Client.CreatePlayer(new(playerName));
        LocalState.AddPlayer(playerName, playerId);
        Renderer.Render<PickUserScreen>();
    }
}