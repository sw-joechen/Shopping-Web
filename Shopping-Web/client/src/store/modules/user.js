import Cookies from 'js-cookie';
import { DateTime } from 'luxon';

const state = () => ({
  account: Cookies.get('admin_account') || '',
  isLoggedIn: Cookies.get('admin_account') ? true : false,
});

const getters = {};

const actions = {};

const mutations = {
  setUser(state, payload) {
    const { account } = payload;
    state.account = account;
    state.isLoggedIn = true;

    // TODO: 有效時間拉到server給
    const expiredDatetime = DateTime.now().plus({ day: 7 }).toISO();
    Cookies.set('admin_account', account, {
      expires: new Date(expiredDatetime),
    });
  },
  clearUser(state) {
    state.account = '';
    state.isLoggedIn = false;
    Cookies.remove('admin_account');
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
