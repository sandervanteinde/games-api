using Spectre.Console.Rendering;

namespace GameApis.ConsolePlayer.Screens;

public class PickUserScreen : IScreen
{
    private const string newPlayerChoice = "[red]New player[/]";
    public void Render()
    {
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Which player would you like to be?")
                .AddChoices(LocalState.GetPlayerNames())
                .AddChoices(newPlayerChoice)
        );

        if (selection is newPlayerChoice)
        {
            Renderer.Render<CreateNewPlayerScreen>();
        }
    }
}