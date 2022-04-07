<template>
  <div class="card flex cursor-pointer">
    <div class="wrapper w-full rounded shadow-lg flex flex-col overflow-hidden">
      <div
        class="imgContainer relative w-full pt-[100%] bg-white overflow-hidden"
      >
        <!--  TODO: svg可以放在background -->
        <img
          src="@/assets/spin_animated.svg"
          class="w-20 h-20 absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2"
        />
        <img
          :class="`img${product.id}`"
          class="img w-full h-full absolute top-0 left-0 object-contain align-bottom hover:scale-125 transition duration-300"
          :data-temp="redirectImg(product.picture)"
        />
      </div>
      <div class="content flex-grow flex flex-col">
        <div
          :title="product.name"
          class="name txtEllipsis font-bold text-2xl mb-2 text-left px-5"
        >
          {{ product.name }}
        </div>
        <div
          :title="product.description"
          class="desc txtEllipsis text-gray-700 text-base flex-grow text-left px-5"
        >
          {{ product.description }}
        </div>
        <div class="amount flex justify-end mt-1 px-5">
          <div>剩餘數量 {{ product.amount }}</div>
        </div>
        <div class="bottom flex px-1 py-2">
          <div class="price leading-10 text-xl text-red2 mr-2">
            <span class="text-base">$</span>{{ product.price }}
          </div>
          <div class="btnsWrapper flex justify-between flex-grow">
            <BtnPrimary
              theme="green"
              class="whitespace-nowrap"
              label="加入購物車"
              @submit="AddToCartHandler"
              px="px-2"
            />
            <BtnPrimary
              label="結帳"
              @submit="CheckoutHandler"
              theme="red"
              px="px-3"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BtnPrimary from '@/components/BtnPrimary.vue';
export default {
  name: 'cardView',
  components: {
    BtnPrimary,
  },
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
    AddToCartHandler() {
      this.$store.commit('shoppingCart/AddProduct', {
        ...this.product,
      });
      this.$store.commit('eventBus/Push', {
        type: 'success',
        content: '加入購物車成功',
      });
    },
    CheckoutHandler() {
      this.AddToCartHandler();
      this.$router.push({ name: 'shoppingCart' });
    },
    redirectImg(path) {
      if (process.env.NODE_ENV === 'development') return path;
      return `http://localhost:15770${path}`;
    },
  },
};
</script>

<style lang="scss">
.card {
  &:hover .wrapper {
    @apply -translate-y-1 transition duration-200 shadow-2xl;
  }
  .txtEllipsis {
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
    overflow: hidden;
  }
}
</style>
