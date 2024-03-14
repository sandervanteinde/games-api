using GameApis.Games.TicTacToe.Models;
using GameApis.Shared.GameState;

namespace GameApis.Games.TicTacToe.Actions;

public record StartGameAction(PlayerTurn? PlayerStarting) : IAction;