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
            class="register px-[30px] py-3 bg-white text-black3 rounded-md h-[450px] w-[24rem] absolute m-auto top-0 bottom-0 left-0 right-0"
          >
            <div class="title mb-2 text-xl text-left">
              {{ $t(`common.${option}`) }}
            </div>

            <!-- 帳號 -->
            <div class="mb-[18px]">
              <input
                :class="{ '!border-red-600': isAccountWarning }"
                v-model="registerData.account"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
                type="text"
                :placeholder="$t('common.account')"
                @keyup.enter="SubmitHandler"
                @focus="isAccountWarning = false"
              />
            </div>

            <!-- 密碼 -->
            <div class="mb-[18px]">
              <input
                :class="{ '!border-red-600': isPwdWarning }"
                v-model="registerData.pwd"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
                type="password"
                :placeholder="$t('common.pwd')"
                @keyup.enter="SubmitHandler"
                @focus="isPwdWarning = false"
              />
            </div>

            <!-- 地址 -->
            <div class="mb-[18px]" v-if="option === 'register'">
              <input
                :class="{ '!border-red-600': isAddressWarning }"
                v-model="registerData.address"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
                type="text"
                :placeholder="$t('common.address')"
                @keyup.enter="SubmitHandler"
                @focus="isAddressWarning = false"
              />
            </div>

            <!-- 電話 -->
            <div class="mb-[18px]" v-if="option === 'register'">
              <input
                :class="{ '!border-red-600': isPhoneWarning }"
                v-model="registerData.phone"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
                type="number"
                :placeholder="$t('common.phone')"
                @keyup.enter="SubmitHandler"
                @keypress="keypressHandler"
                @focus="isPhoneWarning = false"
              />
            </div>

            <!-- 性別 -->
            <div class="mb-2 flex" v-if="option === 'register'">
              <div class="flex items-center mr-4">
                <input
                  id="male"
                  type="radio"
                  :value="1"
                  class="hidden peer"
                  v-model="registerData.gender"
                  checked
                />
                <label
                  for="male"
                  class="flex items-center cursor-pointer peer-checked:text-green6"
                >
                  <span
                    class="w-4 h-4 inline-block mr-1 border border-grey transition-all duration-200"
                    :class="{
                      'bg-green6 shadow-radioBtn1': registerData.gender === 1,
                    }"
                  />
                  男性
                </label>
              </div>
              <div class="flex items-center mr-4">
                <input
                  id="female"
                  type="radio"
                  :value="0"
                  class="hidden"
                  v-model="registerData.gender"
                />
                <label for="female" class="flex items-center cursor-pointer">
                  <span
                    :class="{
                      'bg-green6 shadow-radioBtn1': registerData.gender === 0,
                    }"
                    class="w-4 h-4 inline-block mr-1 border border-grey"
                  />
                  女性
                </label>
              </div>
            </div>

            <!-- email -->
            <div class="mb-[18px]" v-if="option === 'register'">
              <input
                :class="{ '!border-red-600': isEmailWarning }"
                v-model="registerData.email"
                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
                type="text"
                :placeholder="$t('common.email')"
                @keyup.enter="SubmitHandler"
                @focus="isEmailWarning = false"
              />
            </div>
            <div class="flex items-center justify-center">
              <BtnLogin
                :label="$t(`common.${option}`)"
                @submit="SubmitHandler"
                :width="'w-full'"
              />
            </div>

            <hr class="my-3" />

            <div class="policy mb-5" v-if="option === 'login'">
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
import {
  IsAccountValid,
  IsPwdValid,
  IsPureNumber,
  IsPhoneNumber,
  IsEmailValid,
  IsContaineSpecialCharaters,
} from '@/Utils/validators';
import errorList from '@/ErrorCodeList';

export default {
  name: 'loginAndRegister',
  components: {
    BtnLogin,
  },
  data: () => {
    return {
      option: '',
      registerData: {
        account: '',
        pwd: '',
        address: '',
        phone: '',
        gender: 1,
        email: '',
      },
      isAccountWarning: false,
      isPwdWarning: false,
      isAddressWarning: false,
      isPhoneWarning: false,
      isEmailWarning: false,
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
    SubmitHandler() {
      // if (!this.account.length || !this.pwd.length || !this.address.length || this.) {
      //   this.$store.commit('eventBus/Push', {
      //     type: 'error',
      //     content: '請輸入帳號密碼',
      //   });
      //   return;
      // }
      // if (
      //   IsContaineSpecialCharaters(this.account) ||
      //   IsContaineSpecialCharaters(this.pwd)
      // ) {
      //   this.$store.commit('eventBus/Push', {
      //     type: 'error',
      //     content: '帳號密碼不允許特殊符號',
      //   });
      //   return;
      // }
      if (this.option === 'register') this.RegisterHandler();
      else this.LoginHandler();
    },
    async RegisterHandler() {
      const isAccountValid = IsAccountValid(this.registerData.account);
      if (!isAccountValid) this.isAccountWarning = true;

      const isPwdValid = IsPwdValid(this.registerData.pwd);
      if (!isPwdValid) this.isPwdWarning = true;

      const isAddressValid = this.CheckString(this.registerData.address);
      if (!isAddressValid) this.isAddressWarning = true;

      const isPhoneValid = IsPhoneNumber(this.registerData.phone);
      if (!isPhoneValid) this.isPhoneWarning = true;

      const isEmailValid = IsEmailValid(this.registerData.email);
      if (!isEmailValid) this.isEmailWarning = true;

      if (
        isAccountValid &&
        isPwdValid &&
        isAddressValid &&
        isPhoneValid &&
        isEmailValid
      ) {
        const fd = new FormData();
        fd.append('account', this.registerData.account);
        fd.append('pwd', this.registerData.pwd);
        fd.append('address', this.registerData.address);
        fd.append('phone', this.registerData.phone);
        fd.append('gender', this.registerData.gender);
        fd.append('email', this.registerData.email);

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
      }
    },
    async LoginHandler() {
      const isAccountValid = IsContaineSpecialCharaters(
        this.registerData.account
      );
      if (!isAccountValid) this.isAccountWarning = true;

      const isPwdValid = IsContaineSpecialCharaters(this.registerData.pwd);
      if (!isPwdValid) this.isPwdWarning = true;

      if (isAccountValid && isPwdValid) {
        const fd = new FormData();
        fd.append('account', this.registerData.account);
        fd.append('pwd', this.registerData.pwd);

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
      this.registerData.account = '';
      this.registerData.pwd = '';
      this.registerData.address = '';
      this.registerData.phone = '';
      this.registerData.gender = 1;
      this.registerData.email = '';
    },

    keypressHandler(event) {
      return event.charCode >= 48 && event.charCode <= 57
        ? true
        : event.preventDefault();
    },

    // 檢查字串不包含特殊字元, 空白
    CheckString(str) {
      return str && !IsContaineSpecialCharaters(str);
    },

    // 檢查數字不包含小數點, 空白
    CheckNumber(number) {
      return IsPureNumber(number);
    },

    // 檢查字串長度
    CheckStringLength(str, length) {
      if (!str) return false;
      return str.length <= length;
    },
  },
};
</script>

<style lang="scss">
.loginAndRegister {
  input::-webkit-outer-spin-button,
  input::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }

  input[type='radio'] + label span:hover,
  input[type='radio'] + label:hover span {
    transform: scale(1.2);
  }
}
</style>
