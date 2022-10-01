import axios from 'axios';
import React from 'react';

interface State {
  games: string[] | null;
}
export class GamesLobby extends React.Component<{}, State> {
  constructor(props: {}) {
    super(props);
    this.state = {
      games: null
    };
  }


  render(): React.ReactNode {
    const { games } = this.state;
    if(games === null) {
      return <>Loading...</>
    }

    const items = games.map(game => <div key={game}>{game}</div>);

    return (<div style={{display: 'flex'}}>
      {items}
    </div>);
  }

  async componentDidMount(): Promise<void> {
    const gamesResponse = await axios.get<string[]>('/api/games');
    this.setState({
      games: gamesResponse.data
    });
  }
}
