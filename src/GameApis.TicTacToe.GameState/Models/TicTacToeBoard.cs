namespace GameApis.TicTacToe.GameState.Models;

public class TicTacToeBoard
{
    public BoardState TopLeft { get; set; }
    public BoardState Top { get; set; }
    public BoardState TopRight { get; set; }
    public BoardState Left { get; set; }
    public BoardState Middle { get; set; }
    public BoardState Right { get; set; }
}
