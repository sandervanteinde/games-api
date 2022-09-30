import React from "react";
import ReactDOM from "react-dom/client";
import { Board } from './Board';
import './index.scss';


type Item = null | 'X' | 'O';
interface State {
  xIsNext: boolean;
  history: Array<{ squares: Item[]; }>;
  stepNumber: number;
}
class Game extends React.Component<{}, State> {
  constructor(props: {}) {
    super(props);
    this.state = {
      xIsNext: true,
      history: [
        {
          squares: Array.from({ length: 9 }).fill(null) as Item[]
        }
      ],
      stepNumber: 0
    };
  }
  render() {
    const history = this.state.history;
    const current = history[this.state.stepNumber];
    const winner = this.determineWinner(current.squares);
    const status = winner === null
      ? `Next player: ${this.state.xIsNext ? 'X' : 'O'}`
      : `The winner is ${winner}`;

    const moves = history.map((step, move) => {
      const desc = move ? `Go to move #${move}` : 'Go to start';
      return (
        <li key={move}>
          <button onClick={() => this.jumpTo(move)}>{desc}</button>
        </li>
      );
    });
    return (
      <div className="game">
        <div className="game-board">
          <Board
            squares={current.squares}
            onClick={val => this.onClickTile(val)}
          />
        </div>
        <div className="game-info">
          <div>{status}</div>
          <ol>{moves}</ol>
        </div>
      </div>
    );
  }

  onClickTile(value: number): void {
    const history = this.state.history.slice(0, this.state.stepNumber + 1);
    const current = history[history.length - 1];
    if (current.squares[value] !== null || this.determineWinner(current.squares) !== null) {
      return;
    }

    const clone = [...current.squares];
    clone[value] = this.state.xIsNext ? 'X' : 'O';
    this.setState({
      history: [...history, { squares: clone }],
      xIsNext: !this.state.xIsNext,
      stepNumber: history.length
    });
  }

  private jumpTo(move: number): void {
    this.setState({
      stepNumber: move,
      xIsNext: move % 2 === 0
    });
  }

  private determineWinner(squares: Item[]): Item {
    const lines = [
      [0, 1, 2],
      [3, 4, 5],
      [6, 7, 8],
      [0, 3, 6],
      [1, 4, 7],
      [2, 5, 8],
      [0, 4, 8],
      [2, 4, 6],
    ];

    for (let i = 0; i < lines.length; i++) {
      const [a, b, c] = lines[i];
      if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c]) {
        return squares[a];
      }
    }
    return null;
  }
}

// ========================================
// @ts-ignore
const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<Game />);
