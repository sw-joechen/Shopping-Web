<template>
  <div class="indexView">
    <div class="cardsContainer grid grid-cols-5 gap-5 max-w-[1280px] mx-auto">
      <Card v-for="item in productList" :key="item.id" :product="item" />
    </div>
  </div>
</template>

<script>
import Card from '@/components/Card.vue';
import { GetProductList } from '@/APIs/product';
import errorList from '@/ErrorCodeList';

export default {
  components: {
    Card,
  },
  data: () => {
    return {
      productList: [],
    };
  },
  async created() {
    const res = await GetProductList();
    if (res.code === 200) {
      this.productList = res.data;
    } else {
      this.$store.commit('eventBus/Push', {
        type: 'error',
        content: errorList[res.code],
      });
    }
  },
};
</script>

<style></style>
