<template>
  <div class="layoutView p-2 min-h-screen bg-black4">
    <NavBar @toggle="ToggleHandler" />
    <router-view />

    <ConfirmDialog
      v-if="isShowLogOutDialog"
      :is-show-dialog="isShowLogOutDialog"
      :title="$t('navBar.confirmLogout')"
      @toggle="ToggleHandler"
      @submit="LogoutHandler"
    />
  </div>
</template>

<script>
import NavBar from '@/components/NavBar.vue';
import ConfirmDialog from '@/components/Dialogs/DialogView.vue';
export default {
  name: 'layoutView',
  components: {
    NavBar,
    ConfirmDialog,
  },
  data: () => {
    return {
      isShowLogOutDialog: false,
    };
  },
  methods: {
    ToggleHandler() {
      this.isShowLogOutDialog = !this.isShowLogOutDialog;
    },

    LogoutHandler() {
      this.ToggleHandler();
      this.$store.commit('user/clearUser');
    },
  },
};
</script>

<style lang="scss"></style>
