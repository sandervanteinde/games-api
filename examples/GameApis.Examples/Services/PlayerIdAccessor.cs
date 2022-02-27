using Blazored.LocalStorage;
using GameApis.Examples.Client;

namespace GameApis.Examples.Services;

public class PlayerIdAccessor : IPlayerIdAccessor
{
    private const string LocalstorageKey = "PlayerId";
    private readonly ILocalStorageService _localStorageService;
    private PlayerId? _playerId;

    public PlayerIdAccessor(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<PlayerId?> GetPlayerIdAsync()
    {
        if (_playerId is not null)
        {
            return _playerId;
        }

        _playerId = await _localStorageService.GetItemAsync<PlayerId?>(LocalstorageKey);
        return _playerId;
    }

    public async Task SetPlayerIdAsync(PlayerId playerId)
    {
        _playerId = playerId;
        await _localStorageService.SetItemAsync(LocalstorageKey, playerId);
    }

    public PlayerId? GetPlayerId()
    {
        return _playerId;
    }
}