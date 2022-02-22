using GameApis.Shared;
using GameApis.Shared.GameState;

namespace GameApis.TicTacToe.GameState.Actions;

public record JoinPlayerAction(PlayerId PlayerId) : IAction;
