namespace GameApis.Shared;

public record struct GameId(Guid Value)
{
    public static GameId New()
    {
        return new(Guid.NewGuid());
    }
};
