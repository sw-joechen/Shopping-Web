import Mock from 'mockjs';

const Login = (payload) => {
  let tempData = {};
  if (payload.body) {
    let account = '';
    for (var pair of payload.body.entries()) {
      if (pair[0] === 'account') account = pair[1];
    }
    tempData = {
      id: 1,
      account,
      address: '',
      phone: '',
      gender: '',
      email: '',
      enabled: 1,
      balance: '',
      createdDate: '2022-03-07T15:41:06.280',
      updatedDate: '2022-03-07T15:41:06.280',
    };
  }
  const result = {
    code: 200,
    msg: 'success',
    data: tempData,
  };
  return JSON.stringify(result);
};

if (process.env.NODE_ENV === 'development') {
  Mock.mock('/api/member/login', Login);
}
