import request from '../Utils/request';

export const GetProductList = (data) => {
  return request({
    method: 'post',
    url: '/api/product/getProductsList',
    data,
  })
    .then((res) => {
      return JSON.parse(res.data);
    })
    .catch((err) => {
      console.log('err: ', err);
    });
};
