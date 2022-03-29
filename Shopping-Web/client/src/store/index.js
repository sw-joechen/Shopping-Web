import Vue from 'vue';
import Vuex from 'vuex';
import user from './modules/user';
import eventBus from './modules/eventBus';
Vue.use(Vuex);

const store = new Vuex.Store({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {
    user,
    eventBus,
  },
});

export default store;
