<template>
  <div class="navBar flex justify-between w-[1400px] mx-auto">
    <div class="wrapper flex items-center">
      <div class="imgContainer mr-1 cursor-pointer" @click="RouteHomeHandler">
        <img class="w-14" src="@/assets/logo.png" alt="阿進購物" />
      </div>
      <div
        class="title text-4xl text-green1 mr-3 cursor-pointer"
        @click="RouteHomeHandler"
      >
        阿進購物
      </div>
    </div>
    <div class="wrapper">
      <div
        v-if="getAccount.length"
        class="welcome px-3 flex justify-end items-center"
      >
        歡迎回來 &nbsp;<span class="text-green1">{{ getAccount }}</span>
      </div>
      <div v-else class="inputsContainer flex justify-end items-center">
        <!-- 帳號 -->
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">
            {{ $t('common.account') }}
          </label>
          <input
            v-model="account"
            type="text"
            class="input"
            :placeholder="$t('common.accountPlaceholder')"
            @keypress.enter="LoginHandler"
          />
        </div>

        <!-- 密碼 -->
        <div class="inputGroup flex">
          <label class="whitespace-nowrap pr-3 leading-[42px]">
            {{ $t('common.pwd') }}
          </label>
          <input
            v-model="pwd"
            type="password"
            class="input"
            :placeholder="$t('common.pwdPlaceholder')"
            @keypress.enter="LoginHandler"
          />
        </div>

        <BtnPrimary
          class="px-[10px]"
          :label="$t('common.login')"
          @submit="LoginHandler"
        />
      </div>
      <div class="linksContainer flex justify-end items-center">
        <router-link :to="{ name: 'info' }" class="links">
          {{ $t('common.personalInfo') }}
        </router-link>
        |
        <router-link :to="{ name: 'shoppingCart' }" class="links">
          {{ $t('common.shoppingCart') }}
        </router-link>
        <div v-if="getAccount.length" class="flex">
          <span>|</span>
          <div class="logout links cursor-pointer" @click="ToggleHandler">
            {{ $t('common.logout') }}
          </div>
        </div>
        <div v-else class="flex">
          <span>|</span>
          <div class="register links cursor-pointer" @click="RouteHandler">
            {{ $t('common.register') }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BtnPrimary from '../components/BtnPrimary.vue';
import { Login } from '../APIs/member';
import errorList from '../ErrorCodeList';
import { IsContaineSpecialCharaters } from '../Utils/validators';
export default {
  name: 'navBar',
  components: {
    BtnPrimary,
  },
  data: () => {
    return {
      account: '',
      pwd: '',
      isShowLogOutDialog: false,
    };
  },
  computed: {
    getAccount() {
      return this.$store.state.user.account || '';
    },
  },
  methods: {
    RouteHomeHandler() {
      this.$router.push({ name: 'home' }).catch(() => {});
    },
    RouteHandler() {
      this.$router.push({
        name: 'login',
        params: {
          option: 'register',
        },
      });
    },
    async LoginHandler() {
      if (!this.account.length || !this.pwd.length) {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '請輸入帳號密碼',
        });
        return;
      }

      if (
        IsContaineSpecialCharaters(this.account) ||
        IsContaineSpecialCharaters(this.pwd)
      ) {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '帳號密碼不允許特殊符號',
        });
        return;
      }
      const fd = new FormData();
      fd.append('account', this.account);
      fd.append('pwd', this.pwd);

      const res = await Login(fd);
      if (res.code === 200) {
        this.ClearInputs();

        this.$store.commit('user/setUser', {
          account: res.data.account,
        });
        this.$store.commit('eventBus/Push', {
          type: 'success',
          content: this.$t('common.success'),
        });
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: errorList[res.code],
        });
      }
    },
    ToggleHandler() {
      this.$emit('toggle');
    },
    ClearInputs() {
      this.account = '';
      this.pwd = '';
    },
  },
};
</script>

<style lang="scss">
.navBar {
  .inputGroup {
    @apply p-3 pl-0;
    .input {
      @apply bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-green1 focus:ring-1 focus:border-green1 block w-full p-2.5 outline-none;
    }
  }
  .links {
    @apply px-[10px] hover:bg-green1 hover:text-white transition-all;
  }
}
</style>
