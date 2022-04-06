<template>
  <div class="personalInfo flex justify-center items-center">
    <div class="wrapper w-[400px] h-[600px]">
      <!-- 帳號 -->
      <div class="inputGroup mb-[18px] relative flex">
        <label class="absolute leading-[38px]">
          {{ $t('common.account') }}
        </label>
        <input
          v-model="queryInfos.account"
          class="pointer-events-none ml-20 w-full shadow border rounded py-2 px-3 text-gray-700 leading-tight"
          type="text"
          disabled
          readonly
        />
      </div>

      <!-- 餘額 -->
      <div class="inputGroup mb-[18px] relative flex">
        <label class="absolute leading-[38px]">
          {{ $t('common.balance') }}
        </label>
        <input
          v-model="queryInfos.balance"
          class="pointer-events-none ml-20 w-full shadow border rounded py-2 px-3 text-gray-700 leading-tight"
          type="text"
          disabled
          readonly
        />
      </div>

      <!-- 地址 -->
      <div class="inputGroup mb-[18px] relative flex">
        <label class="absolute leading-[38px]">
          {{ $t('common.address') }}
        </label>
        <input
          :class="{ '!border-red-600': isAddressWarning }"
          @focus="isAddressWarning = false"
          v-model="queryInfos.address"
          class="ml-20 w-full shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
          type="text"
          @keyup.enter="SubmitHandler"
        />
        <p
          v-show="isAddressWarning"
          class="text-red-500 text-xs italic absolute -bottom-4 left-20"
        >
          {{ $t('loginAndRegister.addressWarning') }}
        </p>
      </div>

      <!-- 電話 -->
      <div class="inputGroup mb-[18px] relative flex">
        <label class="absolute leading-[38px]">
          {{ $t('common.phone') }}
        </label>
        <input
          :class="{ '!border-red-600': isPhoneWarning }"
          @focus="isPhoneWarning = false"
          v-model="queryInfos.phone"
          class="ml-20 w-full shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
          type="text"
          @keyup.enter="SubmitHandler"
        />
        <p
          v-show="isPhoneWarning"
          class="text-red-500 text-xs italic absolute -bottom-4 left-20"
        >
          {{ $t('loginAndRegister.phoneWarning') }}
        </p>
      </div>

      <!-- email -->
      <div class="inputGroup mb-[18px] relative flex">
        <label class="absolute leading-[38px]">
          {{ $t('common.email') }}
        </label>
        <input
          :class="{ '!border-red-600': isEmailWarning }"
          @focus="isEmailWarning = false"
          v-model="queryInfos.email"
          class="ml-20 w-full shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
          type="text"
          @keyup.enter="SubmitHandler"
        />
        <p
          v-show="isEmailWarning"
          class="text-red-500 text-xs italic absolute -bottom-4 left-20"
        >
          {{ $t('loginAndRegister.emailWarning') }}
        </p>
      </div>
      <div class="btnsWrapper flex justify-end relative">
        <BtnPrimary
          theme="red"
          label="修改密碼"
          @submit="ToggleEditDialogHandler"
          class="absolute left-0"
        />
        <BtnPrimary label="取消" theme="white" class="mr-3" />
        <BtnPrimary label="確認修改" @submit="SubmitHandler" theme="green" />
      </div>
    </div>
    <FormDialog
      title="修改密碼"
      v-if="isShowEditDialog"
      :isShowDialog="isShowEditDialog"
      @toggle="ToggleEditDialogHandler"
      @submit="SubmitEditPwdHandler"
    >
      <div class="wrapper">
        <!-- 舊密碼 -->
        <div class="inputGroup mb-5 relative flex">
          <label class="absolute leading-[38px]"> 舊密碼 </label>
          <input
            type="password"
            :class="{ '!border-red-600': isOldPwdWarning }"
            @focus="isOldPwdWarning = false"
            v-model="editPwd.oldPwd"
            class="ml-20 w-full shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
          />
          <p
            v-show="isOldPwdWarning"
            class="text-red-500 text-xs italic absolute -bottom-4 left-20"
          >
            密碼格式錯誤
          </p>
        </div>

        <!-- 新密碼 -->
        <div class="inputGroup mb-5 relative flex">
          <label class="absolute leading-[38px]"> 新密碼 </label>
          <input
            type="password"
            :class="{ '!border-red-600': isNewPwdWarning }"
            @focus="isNewPwdWarning = false"
            v-model="editPwd.newPwd"
            class="ml-20 w-full shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
          />
          <p
            v-show="isNewPwdWarning"
            class="text-red-500 text-xs italic absolute -bottom-4 left-20 whitespace-nowrap"
          >
            {{ $t('loginAndRegister.pwdWarning') }}
          </p>
        </div>

        <!-- 確認密碼 -->
        <div class="inputGroup mb-5 relative flex">
          <label class="absolute leading-[38px]"> 確認密碼 </label>
          <input
            :class="{ '!border-red-600': isConfirmPwdWarning }"
            @focus="isConfirmPwdWarning = false"
            v-model="editPwd.confirmPwd"
            class="ml-20 w-full shadow appearance-none border rounded py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:ring-green1 focus:ring-1 focus:border-green1"
            type="password"
          />
          <p
            v-show="isConfirmPwdWarning"
            class="text-red-500 text-xs italic absolute -bottom-4 left-20"
          >
            密碼不相同
          </p>
        </div>
      </div>
    </FormDialog>
  </div>
</template>

<script>
import {
  GetMemberPersonalInfo,
  UpdateMember,
  UpdateMemberPwd,
} from '@/APIs/member';
import BtnPrimary from '@/components/BtnPrimary.vue';
import errorList from '@/ErrorCodeList';
import {
  IsPhoneNumber,
  IsEmailValid,
  IsPwdValid,
  IsContaineSpecialCharaters,
} from '@/Utils/validators';
import FormDialog from '@/components/Dialogs/DialogView.vue';
export default {
  name: 'personalInfo',
  components: {
    BtnPrimary,
    FormDialog,
  },
  data() {
    return {
      queryInfos: {
        account: this.$store.state.user.account,
        balance: null,
        address: '',
        phone: '',
        email: '',
        gender: 1,
        enabled: 1,
      },
      oldInfos: {
        account: '',
        address: '',
        phone: '',
        email: '',
        gender: 1,
        enabled: 1,
      },
      editPwd: {
        oldPwd: '',
        newPwd: '',
        confirmPwd: '',
      },
      isAccountWarning: false,
      isAddressWarning: false,
      isPhoneWarning: false,
      isEmailWarning: false,

      isOldPwdWarning: false,
      isNewPwdWarning: false,
      isConfirmPwdWarning: false,
      isShowEditDialog: false,
    };
  },
  async created() {
    const fd = new FormData();
    fd.append('account', this.$store.state.user.account);

    const res = await GetMemberPersonalInfo(fd);
    if (res.code === 200 && res.data.length) {
      this.queryInfos.address = res.data[0].address;
      this.queryInfos.phone = res.data[0].phone;
      this.queryInfos.email = res.data[0].email;
      this.queryInfos.gender = res.data[0].gender;
      this.queryInfos.enabled = res.data[0].enabled;
      this.queryInfos.balance = res.data[0].balance;

      // 備份原始資料
      this.oldInfos = JSON.parse(JSON.stringify(this.queryInfos));
    } else {
      this.$store.commit('eventBus/Push', {
        type: 'error',
        content: errorList[res.code],
      });
    }
  },
  methods: {
    async SubmitEditPwdHandler() {
      // 確認舊密碼格式
      const isOldPwdValid = this.CheckString(this.editPwd.oldPwd);
      if (!isOldPwdValid) this.isOldPwdWarning = true;

      // 確認新密碼格式
      const isNewPwdValid = IsPwdValid(this.editPwd.newPwd);
      if (!isNewPwdValid) this.isNewPwdWarning = true;

      // 比對新密碼&確認密碼
      const isPwdConfirmed =
        this.editPwd.newPwd === this.editPwd.confirmPwd &&
        this.CheckString(this.editPwd.confirmPwd);
      if (!isPwdConfirmed) this.isConfirmPwdWarning = true;

      // 發req
      if (isOldPwdValid && isNewPwdValid && isPwdConfirmed) {
        const fd = new FormData();
        fd.append('account', this.oldInfos.account);
        fd.append('oldPwd', this.editPwd.oldPwd);
        fd.append('newPwd', this.editPwd.newPwd);
        const res = await UpdateMemberPwd(fd);
        if (res.code === 200) {
          this.ClearEditPwd();
          this.ToggleEditDialogHandler();
          this.$store.commit('eventBus/Push', {
            type: 'success',
            content: '修改成功',
          });
        } else {
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: errorList[res.code],
          });
        }
        console.log('res=> ', res);
      }
    },
    ToggleEditDialogHandler() {
      this.ClearEditPwd();
      this.isShowEditDialog = !this.isShowEditDialog;
    },
    async SubmitHandler() {
      const isAddressValid = this.CheckString(this.queryInfos.address);
      if (!isAddressValid) this.isAddressWarning = true;

      const isPhoneValid = IsPhoneNumber(this.queryInfos.phone);
      if (!isPhoneValid) this.isPhoneWarning = true;

      const isEmailValid = IsEmailValid(this.queryInfos.email);
      if (!isEmailValid) this.isEmailWarning = true;

      if (isAddressValid && isPhoneValid && isEmailValid) {
        const fd = new FormData();
        fd.append('account', this.queryInfos.account);
        fd.append('address', this.queryInfos.address);
        fd.append('phone', this.queryInfos.phone);
        fd.append('gender', this.queryInfos.gender);
        fd.append('email', this.queryInfos.email);

        try {
          const res = await UpdateMember(fd);
          if (res.code === 200) {
            this.$store.commit('eventBus/Push', {
              type: 'success',
              content: '修改成功',
            });
          } else {
            this.$store.commit('eventBus/Push', {
              type: 'error',
              content: errorList[res.code],
            });
          }
        } catch (e) {
          this.$store.commit('eventBus/Push', {
            type: 'error',
            content: errorList[101],
          });
        }
      }
    },

    // 檢查字串不包含特殊字元, 空白
    CheckString(str) {
      return str && !IsContaineSpecialCharaters(str);
    },

    ClearEditPwd() {
      this.editPwd.oldPwd = '';
      this.editPwd.newPwd = '';
      this.editPwd.confirmPwd = '';
    },

    ClearWarning() {
      this.isOldPwdWarning = false;
      this.isNewPwdWarning = false;
      this.isConfirmPwdWarning = false;
    },
  },
};
</script>

<style lang="scss"></style>
