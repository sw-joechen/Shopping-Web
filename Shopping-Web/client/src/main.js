import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';
import './index.css';
import VueI18n from 'vue-i18n';
import tw from './locales/tw';

Vue.use(VueI18n);
Vue.config.productionTip = false;

const messages = {
  tw,
};

const i18n = new VueI18n({
  locale: 'tw',
  messages,
});

new Vue({
  router,
  store,
  i18n,
  render: (h) => h(App),
}).$mount('#app');
