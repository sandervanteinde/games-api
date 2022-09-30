import { Square } from './Square';
import './Board.scss';

type Item = null | 'X' | 'O';

interface Props {
  onClick(tile: number): void;
  squares: Array<Item>;
}

export function Board(props: Props) {
  const renderSquare = (index: number) => (
    <Square
      value={props.squares[index]}
      onClick={() => props.onClick(index)}
    />
  );
  return (
    <div>
      <div className="board-row">
        {renderSquare(0)}
        {renderSquare(1)}
        {renderSquare(2)}
      </div>
      <div className="board-row">
        {renderSquare(3)}
        {renderSquare(4)}
        {renderSquare(5)}
      </div>
      <div className="board-row">
        {renderSquare(6)}
        {renderSquare(7)}
        {renderSquare(8)}
      </div>
    </div>
  );
}
