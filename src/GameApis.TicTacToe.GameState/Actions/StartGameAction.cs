using GameApis.Shared.GameState;
using GameApis.TicTacToe.GameState.Models;

namespace GameApis.TicTacToe.GameState.Actions;

public record StartGameAction(PlayerTurn? PlayerStarting) : IAction;
