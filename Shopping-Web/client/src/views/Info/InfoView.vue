<template>
  <div class="infoView flex">
    <div class="menu mr-2">
      <div class="links flex flex-col h-[600px] leading-[38px]">
        <router-link
          class="px-2 text-green7 font-bold hover:text-white hover:bg-green1 transition-colors duration-300;"
          :class="{
            'bg-green1 !text-white': IsCurrentRouteActive(route.to.name),
          }"
          v-for="route in routes"
          :key="route.name"
          :to="route.to.name"
        >
          {{ route.name }}
        </router-link>
      </div>
    </div>
    <router-view class="flex-grow" />
  </div>
</template>

<script>
export default {
  name: 'infoView',
  data() {
    return {
      routes: [
        { name: '修改會員資料', to: { name: 'editInfo' } },
        { name: '購買紀錄', to: { name: 'purchaseHistory' } },
      ],
    };
  },
  methods: {
    IsCurrentRouteActive(route) {
      if (route === this.$route.name) {
        return true;
      }
      return false;
    },
  },
};
</script>

<style lang="scss"></style>
