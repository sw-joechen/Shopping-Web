<template>
  <div class="card rounded overflow-hidden shadow-lg">
    <div class="imgContainer relative w-full pt-[100%]">
      <img
        class="w-full h-full absolute top-0 left-0 object-contain align-bottom"
        :src="redirectImg(product.picture)"
        loading="lazy"
      />
    </div>
    <div class="px-5 py-2">
      <div
        :title="product.name"
        class="name txtEllipsis font-bold text-2xl mb-2"
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
  methods: {
    redirectImg(path) {
      if (process.env.NODE_ENV === 'development') return path;
      return `http://localhost:15770${path}`;
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
