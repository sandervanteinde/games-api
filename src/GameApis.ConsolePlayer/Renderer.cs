using GameApis.ConsolePlayer.Screens;
using System.Threading.Channels;

namespace GameApis.ConsolePlayer;

public static class Renderer
{
    private static Channel<IAsyncScreen> _currentScreen = Channel.CreateUnbounded<IAsyncScreen>();
    public static void Render<TScreen>()
        where TScreen : IAsyncScreen, new()
    {
        _currentScreen.Writer.TryWrite(new TScreen());
    }

    public static async Task Start()
    {
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += delegate
        {
            _currentScreen.Writer.Complete();
            cts.Cancel();
        };

        var reader = _currentScreen.Reader;
        while (!cts.IsCancellationRequested)
        {
            if (!await reader.WaitToReadAsync(cts.Token))
            {
                continue;
            }

            var newScreen = await reader.ReadAsync(cts.Token);
            await newScreen.RenderAsync();
        }
    } 
}