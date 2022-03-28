import Mock from 'mockjs';

const Login = () => {
  const result = {
    code: 200,
    msg: 'success',
    data: {
      account: 'a12345',
      role: '',
    },
  };
  return JSON.stringify(result);
};

if (process.env.NODE_ENV === 'development') {
  Mock.mock('/api/member/login', Login);
}
