namespace GameApis.ConsolePlayer.Screens;

public interface IScreen : IAsyncScreen
{
    void Render();

    Task IAsyncScreen.RenderAsync()
    {
        Render();
        return Task.CompletedTask;
    }
}

public interface IAsyncScreen
{
    
    Task RenderAsync();
}