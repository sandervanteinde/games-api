import './Square.scss';

interface Props {
  value: null | 'X' | 'O';
  onClick: () => void;
};

export function Square(props: Props): JSX.Element {
  return (
    <button
      className="square"
      onClick={() => props.onClick()}
    >
      {props.value}
    </button>
  );
}
