using Refit;

namespace GameApis.ConsolePlayer;

public class Api
{
    public static readonly IGameApiClient Client = RestService.For<IGameApiClient>("http://localhost:5000");
}