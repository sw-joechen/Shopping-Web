import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '@/store/index';
Vue.use(VueRouter);

const routes = [
  {
    path: '/',
    name: 'layout',
    component: () => import('../views/LayoutView.vue'),
    redirect: '/home',
    children: [
      {
        path: 'home',
        name: 'home',
        component: () => import('../views/HomeView.vue'),
      },
    ],
  },
  {
    path: '/login',
    name: 'login',
    beforeEnter: (to, from, next) => {
      if (store.state.user.isLoggedIn) {
        next({ name: 'home' });
      } else {
        next();
      }
    },
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ '../views/LoginAndRegister.vue'),
  },
];

const router = new VueRouter({
  routes,
});

export default router;
