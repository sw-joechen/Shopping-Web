import Mock from 'mockjs';

const Register = () => {
  const result = {
    code: 200,
    msg: 'success',
  };
  return JSON.stringify(result);
};

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

const GetProductList = () => {
  const tempData = [];

  for (let i = 0; i < 50; i++) {
    const start = Math.floor(Math.random() * 10);
    tempData.push({
      id: i,
      name: 'a aaaabb b bbaaaaab bbbba aa abbbbbaaaaa bbbbbaaa aabbbbb',
      amount: 1,
      createdDate: '2022-03-17T15:43:55.653',
      description:
        'xxx xxyy yyyxxxx xy yyyy xxx xxy y yyyxx xx xyyy yyxxx xxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyy',
      enabled: true,
      picture: `https://picsum.photos/200/200?image=${start + i}`,
      price: 1,
      type: 't1',
      updatedDate: '2022-03-17T15:43:55.653',
    });
  }
  const result = {
    code: 200,
    msg: 'success',
    data: tempData,
  };
  return JSON.stringify(result);
};

if (process.env.NODE_ENV === 'development') {
  // member
  Mock.mock('/api/member/login', Login);

  Mock.mock('/api/member/registerMember', Register);

  // product
  Mock.mock('/api/product/getProductsList', GetProductList);
}
