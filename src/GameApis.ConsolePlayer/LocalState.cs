using GameApis.Shared.Players;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace GameApis.ConsolePlayer;

public class LocalState
{
    private static LocalState? _state;
    private Dictionary<string, PlayerId> _playerIds = new();

    private static readonly string _playerIdJsonPath;

    static LocalState()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var gameApiConsoleFolder = Path.Combine(folder, "GameApiConsole");
        _playerIdJsonPath = Path.Combine(gameApiConsoleFolder, "playerIds.json");

        if (!Directory.Exists(gameApiConsoleFolder))
        {
            _state = new();
            Directory.CreateDirectory(gameApiConsoleFolder);
            return;
        }
    }

    public static void Load()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var gameApiConsoleFolder = Path.Combine(folder, "GameApiConsole");

        if (!Directory.Exists(gameApiConsoleFolder))
        {
            _state = new();
            Directory.CreateDirectory(gameApiConsoleFolder);
            return;
        }
        var state = new LocalState();

        if(File.Exists(_playerIdJsonPath))
        {
            var playerIds = JsonSerializer.Deserialize<Dictionary<string, PlayerId>>(File.ReadAllText(_playerIdJsonPath));
            state._playerIds = playerIds!;
        }
        _state = state;
    }

    [MemberNotNull(nameof(_state))]
    private static void EnsureLoaded()
    {
        if (_state is null)
        {
            throw new InvalidOperationException("Local state not loaded");
        }
    }

    public static void AddPlayer(string playerName, PlayerId playerId)
    {
        EnsureLoaded();
        _state._playerIds[playerName] = playerId;
        File.WriteAllText(_playerIdJsonPath, JsonSerializer.Serialize(_state._playerIds));
    }

    public static IReadOnlyCollection<string> GetPlayerNames()
    {
        EnsureLoaded();
        return _state._playerIds.Keys;
    }
}