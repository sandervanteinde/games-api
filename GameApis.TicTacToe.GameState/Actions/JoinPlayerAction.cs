using GameApis.Shared.GameState;
using GameApis.Shared.Players;

namespace GameApis.TicTacToe.GameState.Actions;

public record JoinPlayerAction(ExternalPlayerId PlayerId) : IAction;
