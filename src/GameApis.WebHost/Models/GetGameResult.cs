namespace GameApis.WebHost.Models;

public record GetGameResult<TGameContext>(TGameContext Game, string State);