<template>
  <div class="homeView">
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
  name: 'homeView',
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
    if (res.code === 200 && res.data.length) {
      res.data.forEach((product) => {
        if (product.enabled) this.productList.push(product);
      });
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
