import { Button, Checkbox, Form, Input } from 'antd';
import Paragraph from 'antd/lib/typography/Paragraph';
import axios from 'axios';
import React from 'react';

interface Props {
  loggedIn(userId: string, remember: boolean): void;
}

interface State {
  requestFailed: boolean;
  doingRequest: boolean;
}

export class Login extends React.Component<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = {
      doingRequest: false,
      requestFailed: false
    };
  }

  render(): React.ReactNode {
    const state = this.state;
    const errorMessage = state.requestFailed
      ? <span style={{ color: 'red' }}>Failed to create user.</span>
      : <></>;

    return (
      <>
        <Paragraph>Please login in order to use the example.</Paragraph>
        <Form
          name={'login'}
          initialValues={{ remember: true }}
          onFinish={formValue => this.onFinish(formValue)}
          disabled={state.doingRequest}
        >
          <Form.Item
            label={'Name'}
            name={'name'}
            rules={[{ required: true, message: 'Please input your username!' }]}
          >
            <Input />
          </Form.Item>

          <Form.Item name="remember" valuePropName="checked" wrapperCol={{ offset: 8, span: 16 }}>
            <Checkbox>Remember me</Checkbox>
          </Form.Item>

          <Form.Item>
            <Button loading={state.doingRequest} type="primary" htmlType="submit">
              Submit
            </Button>
          </Form.Item>
        </Form>
        {errorMessage}
      </>
    );
  }
  private async onFinish(value: { name: string, remember: boolean; }): Promise<void> {
    this.setState({ doingRequest: true });
    try {
      const response = await axios.post<{ internalId: { value: string; }, externalId: { value: string; }; }>('/api/player', { playerName: value.name });
      this.setState({ doingRequest: false });
      this.props.loggedIn(response.data.internalId.value, value.remember);
    } catch {
      this.setState({ requestFailed: false, doingRequest: false });
      return;
    }
  }
}
