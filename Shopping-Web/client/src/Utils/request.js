import axios from 'axios';

const request = axios.create();

request.interceptors.request.use(
  function (res) {
    return res;
  },
  function (error) {
    return Promise.reject(error);
  }
);

request.interceptors.response.use(
  function (response) {
    // Do something with response data
    return response;
  },
  function (error) {
    if (error.response) {
      switch (error.response.status) {
        case 404:
          console.log('你要找的頁面不存在');
          // go to 404 page
          break;
        case 500:
          console.log('程式發生問題');
          // go to 500 page
          break;
        default:
          console.log(error.message);
      }
    }
    if (!window.navigator.onLine) {
      alert('網路出了點問題，請重新連線後重整網頁');
      return;
    }
    return Promise.reject(error);
  }
);

export default request;
