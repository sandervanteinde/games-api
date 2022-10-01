import { Layout } from 'antd';
import { Content, Header } from 'antd/lib/layout/layout';
import Title from 'antd/lib/typography/Title';
import React from 'react';
import { Outlet } from 'react-router-dom';
import { Login } from './login/Login';
import './Root.scss';

interface State {
  userLoginToken: string | null;
}

export class Root extends React.Component<{}, State> {
  constructor(props: {}) {
    super(props);
    this.state = {
      userLoginToken: localStorage.getItem('loginId')
    };
  }
  render() {
    const content = this.state.userLoginToken
      ? <Outlet />
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


