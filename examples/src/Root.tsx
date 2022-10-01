import { Layout } from 'antd';
import { Content, Header } from 'antd/lib/layout/layout';
import Title from 'antd/lib/typography/Title';
import axios from 'axios';
import React from 'react';
import { Outlet } from 'react-router-dom';
import { Login } from './login/Login';
import './Root.scss';

interface State {
  userLoginToken: string | null;
  isLoginTokenValidated: boolean;
}

export class Root extends React.Component<{}, State> {
  constructor(props: {}) {
    super(props);
    this.state = {
      userLoginToken: localStorage.getItem('loginId'),
      isLoginTokenValidated: false
    };
    axios.interceptors.request.use(request => {
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
    const content = this.state.userLoginToken
      ? (this.state.isLoginTokenValidated
        ? <Outlet />
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

  async componentDidUpdate(prevProps: Readonly<{}>, prevState: Readonly<State>, snapshot?: any): Promise<void> {
    this.checkValidToken();
  }

  componentDidMount(): void {
    this.checkValidToken();
  }

  private async checkValidToken() {
    const { isLoginTokenValidated, userLoginToken } = this.state;
    if (isLoginTokenValidated || !userLoginToken) {
      return;
    }

    try {
      await axios.get('https://localhost:5001/api/player/me');
      this.setState({ isLoginTokenValidated: true });
    } catch {
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


