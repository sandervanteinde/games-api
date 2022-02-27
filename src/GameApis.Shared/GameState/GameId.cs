namespace GameApis.Shared.GameState;

public record struct GameId(Guid Value)
{
    public static GameId New()
    {
        return new GameId(Guid.NewGuid());
    }
};
