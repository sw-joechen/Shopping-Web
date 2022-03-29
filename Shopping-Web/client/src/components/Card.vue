<template>
  <div class="card rounded overflow-hidden shadow-lg">
    <div class="imgContainer relative w-full pt-[100%] bg-white">
      <img
        src="@/assets/spin_animated.svg"
        class="w-20 h-20 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2"
      />
      <img
        :class="`img${product.id}`"
        class="img w-full h-full absolute top-0 left-0 object-contain align-bottom"
        :data-temp="redirectImg(product.picture)"
      />
    </div>
    <!-- <div class="ob h-[1px] w-full"></div> -->
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
  data: () => {
    return {
      observer: null,
      src: '',
    };
  },
  mounted() {
    this.observer = new IntersectionObserver((entries, observer) => {
      entries.forEach((entry) => {
        //  目標dom進入viewport
        if (entry.isIntersecting) {
          entry.target.setAttribute('src', entry.target.dataset.temp);
          // 完成後，停止觀察dom
          observer.unobserve(entry.target);
        }
      });
    });
    this.observer.observe(document.querySelector(`.img${this.product.id}`));
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
