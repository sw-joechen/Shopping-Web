import Vue from 'vue';
import Vuex from 'vuex';
import user from './modules/user';
import eventBus from './modules/eventBus';
import shoppingCart from './modules/shoppingCart';
Vue.use(Vuex);

const store = new Vuex.Store({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {
    user,
    eventBus,
    shoppingCart,
  },
});

export default store;
