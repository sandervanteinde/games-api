using GameApis.Shared.Players;

namespace GameApis.WebHost.Models;

public record PlayerDto(string Name, ExternalPlayerId ExternalPlayerId);