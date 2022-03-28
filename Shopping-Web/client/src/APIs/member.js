import request from '../Utils/request';

export const Login = (data) => {
  return request({
    method: 'post',
    url: '/api/member/login',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};
