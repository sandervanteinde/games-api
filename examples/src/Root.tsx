import { Layout } from 'antd';
import { Content, Header } from 'antd/lib/layout/layout';
import Title from 'antd/lib/typography/Title';
import axios, { AxiosError } from 'axios';
import React from 'react';
import { Outlet } from 'react-router-dom';
import { Login } from './login/Login';
import { Player } from './models/player';
import './Root.scss';
import { PlayerContext } from './shared-state/player-context';

interface State {
  userLoginToken: string | null;
  playerContext: Player | null;
}

export class Root extends React.Component<{}, State> {
  constructor(props: {}) {
    super(props);
    this.state = {
      userLoginToken: localStorage.getItem('loginId'),
      playerContext: null
    };
    axios.interceptors.request.use(request => {
      request.baseURL = 'https://localhost:5001';
      if (request.method === 'OPTIONS') {
        return request;
      }
      const { userLoginToken } = this.state;
      if (userLoginToken) {
        request.headers = request.headers ?? {};
        request.headers['X-Player-Id'] = userLoginToken;
      }
      return request;
    }, err => Promise.reject(err));
  }

  render() {
    const { playerContext, userLoginToken } = this.state;
    const content = userLoginToken
      ? (playerContext !== null
        ? <PlayerContext.Provider value={playerContext}><Outlet/></PlayerContext.Provider>
        : <div>Loading...</div>)
      : <Login
        loggedIn={(userId, shouldRemember) => this.onLogin(userId, shouldRemember)}
      />;
    return (
      <Layout style={{ width: '100vw', height: '100vh' }}>
        <Header>
          <Title style={{ color: 'white' }}>Games API Examples</Title>
        </Header>
        <Content style={{ padding: '20px' }}>
          {content}
        </Content>
      </Layout>
    );
  }

  componentDidUpdate(): void {
    this.checkValidToken();
  }

  componentDidMount(): void {
    this.checkValidToken();
  }

  private async checkValidToken() {
    const { playerContext, userLoginToken } = this.state;
    if (playerContext !== null || !userLoginToken) {
      return;
    }

    try {
      const playerResponse = await axios.get<Player>('/api/player/me');
      this.setState({ playerContext: playerResponse.data });
    } catch(err: unknown) {
      if(!(err instanceof AxiosError) || err.code === 'ERR_NETWORK') {
        return;
      }
      this.setState({ userLoginToken: null });
      localStorage.removeItem('loginId');
    }
  }

  private onLogin(userId: string, shouldRemember: boolean): void {
    if (shouldRemember) {
      localStorage.setItem('loginId', userId);
    } else {
      localStorage.removeItem('loginId');
    }
    this.setState({
      userLoginToken: userId
    });
  }
}


