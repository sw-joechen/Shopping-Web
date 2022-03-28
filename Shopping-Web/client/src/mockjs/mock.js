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

const GetProductList = () => {
  const result = {
    code: 200,
    msg: 'success',
    data: [
      {
        id: '1',
        name: 'a aaaabb b bbaaaaab bbbba aa abbbbbaaaaa bbbbbaaa aabbbbb',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description:
          'xxx xxyy yyyxxxx xy yyyy xxx xxy y yyyxx xx xyyy yyxxx xxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyy',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: '2',
        name: 'test',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description: 'desc',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: '3',
        amount: 243,
        createdDate: '2022-03-17T15:43:55.653',
        description: 'desc2',
        enabled: false,
        name: 'test2',
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 'type2',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: '4',
        name: 'aaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbb',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description:
          'xxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyy',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },

      {
        id: '5',
        name: 'aaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbb',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description:
          'xxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyy',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: '6',
        name: 'test',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description: 'desc',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: '7',
        amount: 243,
        createdDate: '2022-03-17T15:43:55.653',
        description: 'desc2',
        enabled: false,
        name: 'test2',
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 'type2',
        updatedDate: '2022-03-17T15:43:55.653',
      },
      {
        id: '8',
        name: 'aaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbbaaaaabbbbb',
        amount: 1,
        createdDate: '2022-03-17T15:43:55.653',
        description:
          'xxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyyxxxxxyyyyy',
        enabled: true,
        picture: 'https://picsum.photos/200',
        price: 1,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      },
    ],
  };
  return JSON.stringify(result);
};

if (process.env.NODE_ENV === 'development') {
  // member
  Mock.mock('/api/member/login', Login);

  // product
  Mock.mock('/api/product/getProductsList', GetProductList);
}
