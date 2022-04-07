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

const GetMemberPersonalInfo = (payload) => {
  let tempData = [];
  if (payload.body) {
    let account = '';
    for (var pair of payload.body.entries()) {
      if (pair[0] === 'account') account = pair[1];
    }
    tempData.push({
      id: 1,
      account,
      pwd: '!@!^*&@#$)(FDdwq321',
      address: '台中市blabla',
      phone: '0987654321',
      gender: 1,
      email: 'test@test.com',
      enabled: 1,
      balance: 1.23,
      createdDate: '2022-03-07T15:41:06.280',
      updatedDate: '2022-03-07T15:41:06.280',
    });
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
    tempData.push({
      id: i,
      name:
        getRandom(0, 200) % 2 === 0
          ? '名稱拉；海法馬麼巴備制大利總下方氣形父音？'
          : '就行檢在是後沒陸這奇主，分灣天大山字不，列失裡產人這！是高案民保真像系覺死音消；配連試、同我合些流多',
      amount: 100,
      createdDate: '2022-03-17T15:43:55.653',
      description:
        getRandom(0, 200) % 2 === 0
          ? '描述拉府國想歡飛傳綠已'
          : '卻一裡巴：就行檢在是後沒陸這奇主，分灣天大山字不，列失裡產人這！是高案民保真像系覺死音消；配連試、同我合些流多和……假小情在成旅一認有病有不出產望好孩軍充清……新人找是施輕爸民生雖重不、來了電情消子學上原，題見他是毛是一麼那一創愛治級子還北去。',
      enabled: getRandom(0, 200) % 2 === 0 ? true : false,
      picture: `https://picsum.photos/200/200?image=${getRandom(0, 200)}`,
      price: 10000,
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

//產生min到max之間的亂數
const getRandom = (min, max) => {
  return Math.floor(Math.random() * (max - min + 1)) + min;
};

if (process.env.NODE_ENV === 'development') {
  // member
  Mock.mock('/api/member/login', Login);

  Mock.mock('/api/member/registerMember', Register);

  Mock.mock('/api/member/getMemberPersonalInfo', GetMemberPersonalInfo);

  // product
  Mock.mock('/api/product/getProductsList', GetProductList);
}
