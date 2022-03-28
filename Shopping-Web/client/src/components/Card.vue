<template>
  <div class="card rounded overflow-hidden shadow-lg">
    <img class="w-full" :src="product.picture" />
    <div class="px-6 py-4">
      <div
        :title="product.name"
        class="name txtEllipsis font-bold text-xl mb-2"
      >
        {{ product.name }}
      </div>
      <div
        :title="product.description"
        class="desc txtEllipsis text-gray-700 text-base"
      >
        {{ product.description }}
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'cardView',
  props: {
    // id: number,
    // name: string,
    // description: string,
    // picture: string
    product: {
      required: true,
      type: Object,
    },
  },
};
</script>

<style lang="scss">
.card {
  .txtEllipsis {
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
    overflow: hidden;
  }
}
</style>
