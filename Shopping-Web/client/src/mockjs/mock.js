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
          : '名稱拉一色？天面女回中林成文世原，工學深整友手以現：根媽評力的灣歌動果中足未書內，分力過色靈是希人分……小臉上見果，年西甚務著成很背個命對時意我一別你大家文。',
      amount: 13,
      createdDate: '2022-03-17T15:43:55.653',
      description:
        getRandom(0, 200) % 2 === 0
          ? '描述拉府國想歡飛傳綠已'
          : '描述拉有，一那像風上指李弟生！目式就更首比明心後政，天的設，散兩標；學實包建你品',
      enabled: true,
      picture: `https://picsum.photos/200/200?image=${getRandom(0, 200)}`,
      price: 10,
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
