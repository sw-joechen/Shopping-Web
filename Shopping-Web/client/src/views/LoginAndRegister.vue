<template>
  <div class="loginAndRegister">
    <div class="nav">
      <div
        class="wrapper w-[1200px] h-20 mx-auto flex justify-start items-center"
      >
        <div class="imgContainer mr-1 cursor-pointer" @click="RouteHandler">
          <img class="w-14" src="@/assets/logo.png" alt="阿進購物" />
        </div>
        <div
          class="title text-4xl text-green1 mr-3 cursor-pointer"
          @click="RouteHandler"
        >
          阿進購物
        </div>
        <div class="txt text-2xl">{{ $t(`common.${option}`) }}</div>
      </div>
    </div>
    <div class="main bg-green1">
      <div class="content flex w-[1040px] h-[600px] mx-auto">
        <div class="infos flex-grow text-white flex flex-col justify-center">
          <div class="txt text-6xl font-bold mb-5">-阿進購物-</div>
          <div class="txt text-6xl font-bold mb-5">4/18</div>
          <div class="txt text-6xl font-bold mb-5">免運狂購節</div>
          <div class="txt text-6xl font-bold mb-5">3/26~4/30</div>
        </div>
        <div class="registerContainer w-[400px] relative">
          <div
            class="register p-[30px] bg-white text-black3 rounded-md h-[427px] w-[24rem] absolute m-auto top-0 bottom-0 left-0 right-0"
          >
            <div class="title mb-4 text-xl text-left">
              {{ $t(`common.${option}`) }}
            </div>
            <div class="mb-4">
              <input
                v-model="account"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
                id="username"
                type="text"
                :placeholder="$t('common.account')"
                @keyup.enter="SubmitHandler"
              />
            </div>
            <div class="mb-4">
              <input
                v-model="pwd"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
                id="pwd"
                type="password"
                :placeholder="$t('common.pwd')"
                @keyup.enter="SubmitHandler"
              />
            </div>
            <div class="flex items-center justify-center">
              <BtnLogin
                :label="$t(`common.${option}`)"
                @submit="SubmitHandler"
                :width="'w-full'"
              />
            </div>

            <hr class="my-7" />

            <div class="policy mb-5">
              點擊登入或註冊，即表示您已閱讀並同意阿進購物的
              <span class="text-green1 cursor-pointer">服務條款</span>
              與
              <span class="text-green1 cursor-pointer"> 隱私權政策</span>
            </div>

            <div class="login text-gray1">
              已經有帳號了嗎？
              <span
                class="text-green1 cursor-pointer"
                @click="ChangeOptionHandler"
              >
                {{
                  option === 'register'
                    ? $t('common.login')
                    : $t('common.register')
                }}
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="footer"></div>
  </div>
</template>

<script>
import BtnLogin from '@/components/BtnPrimary.vue';
import { Login, Register } from '@/APIs/member';
import { IsContaineSpecialCharaters } from '@/Utils/validators';
import errorList from '@/ErrorCodeList';

export default {
  name: 'loginAndRegister',
  components: {
    BtnLogin,
  },
  data: () => {
    return {
      account: '',
      pwd: '',
      option: '',
    };
  },
  computed: {
    getParam() {
      return this.$route.params.option;
    },
  },
  created() {
    this.InitOption();
  },
  methods: {
    RouteHandler() {
      this.$router.push({ name: 'home' });
    },
    async SubmitHandler() {
      if (
        !this.account.length ||
        !this.pwd.length ||
        IsContaineSpecialCharaters(this.account) ||
        IsContaineSpecialCharaters(this.pwd)
      ) {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '請輸入帳號密碼',
        });
        return;
      }

      const fd = new FormData();
      fd.append('account', this.account);
      fd.append('pwd', this.pwd);

      // 註冊
      if (this.option === 'register') {
        const res = await Register(fd);
        if (res.code === 200) {
          this.ClearInputs();
          this.$store.commit('eventBus/Push', {
            type: 'success',
            content: '註冊成功',
          });
          this.ChangeOptionHandler();
        } else {
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: errorList[res.code],
          });
        }
      } else {
        // 登入
        const res = await Login(fd);
        if (res.code === 200) {
          this.$store.commit('user/setUser', {
            account: res.data.account,
          });
          this.$router.replace({ name: 'home' });
        } else {
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: errorList[res.code],
          });
        }
      }
    },
    ChangeOptionHandler() {
      if (this.option === 'login') {
        this.option = 'register';
      } else {
        this.option = 'login';
      }
    },
    InitOption() {
      if (this.getParam) {
        this.option = this.getParam;
      } else {
        this.option = 'register';
      }
    },
    ClearInputs() {
      this.account = '';
      this.pwd = '';
    },
  },
};
</script>
