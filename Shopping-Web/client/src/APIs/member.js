import request from '../Utils/request';

export const Register = (data) => {
  return request({
    method: 'post',
    url: '/api/member/registerMember',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

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

export const GetMemberPersonalInfo = (data) => {
  return request({
    method: 'post',
    url: '/api/member/getMemberPersonalInfo',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const UpdateMember = (data) => {
  return request({
    method: 'post',
    url: '/api/member/updateMember',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};

export const UpdateMemberPwd = (data) => {
  return request({
    method: 'post',
    url: '/api/member/updateMemberPwd',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};
