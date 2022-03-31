import Vue from 'vue';
import VueRouter from 'vue-router';
import store from '@/store/index';
import Cookies from 'js-cookie';

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
      {
        path: 'info',
        name: 'info',
        component: () => import('../views/PersonalInfo.vue'),
        meta: {
          isAuthRequired: true,
        },
      },
      {
        path: 'shoppingCart',
        name: 'shoppingCart',
        component: () => import('../views/ShoppingCart.vue'),
        meta: {
          isAuthRequired: true,
        },
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

router.beforeEach(async (to, from, next) => {
  const cookie = Cookies.get('client_account');

  if (to.matched.some((record) => record.meta.isAuthRequired)) {
    if (cookie) {
      next();
    } else {
      // cookie沒東西, 導到登入頁
      next({
        name: 'login',
        params: {
          option: 'login',
        },
      });
    }
  } else {
    next();
  }
});

export default router;
