<template>
  <div class="shoppingCart">
    <BtnPrimary label="demo" @submit="demoHandler" />
    <div class="wrapper max-w-[1200px] mx-auto">
      <div v-for="item in productList" :key="item.id" class="productItem flex">
        <div class="checkbox flex items-center p-2">
          <input type="checkbox" id="checkbox" v-model="item.checked" />
        </div>
        <div class="product flex items-center p-2">
          <div class="imgContainer flex justify-center mr-4">
            <img class="w-16" :src="item.picture" />
          </div>
          <div
            :title="item.description"
            class="break-all flex-grow whitespace-nowrap overflow-hidden text-ellipsis"
          >
            {{ item.description }}
          </div>
        </div>
        <div class="price flex items-center p-2">$ {{ item.price }}</div>
        <div class="amount flex items-center p-2 w-36">
          <div
            class="amount flex h-10 w-full rounded-lg relative bg-transparent"
          >
            <button
              @click="DecrementHandler(item)"
              class="decrement px-3 bg-gray-300 text-gray-600 hover:text-gray-700 hover:bg-gray-400 h-full rounded-l cursor-pointer outline-none"
            >
              <span class="m-auto text-2xl font-thin">−</span>
            </button>
            <input
              v-model="item.amount"
              type="number"
              class="w-full outline-none focus:outline-none text-center bg-gray-300 font-semibold text-md hover:text-black focus:text-black flex items-center text-gray-700"
              @keypress="KeypressHandler"
            />
            <button
              @click="IncrementHandler(item)"
              class="increment px-3 bg-gray-300 text-gray-600 hover:text-gray-700 hover:bg-gray-400 h-full rounded-r cursor-pointer"
            >
              <span class="m-auto text-2xl font-thin">+</span>
            </button>
          </div>
        </div>
        <div class="totalPrice flex items-center p-2">
          $ {{ item.price * item.amount }}
        </div>
        <div class="delete"></div>
      </div>
    </div>
  </div>
</template>

<script>
import BtnPrimary from '@/components/BtnPrimary.vue';
export default {
  name: 'shoppingCart',
  components: {
    BtnPrimary,
  },
  computed: {},
  data() {
    return {
      // id: number
      // name: string
      // amount: number
      // description: string
      // enabled: boolean,
      // picture: string
      // price: number
      // type: string
      // createdDate: string
      // updatedDate:string
      // checked: boolean
      productList: [],
    };
  },
  created() {
    this.$store.commit('shoppingCart/InitShoppingCart');
    this.$store.state.shoppingCart.shoppingCart.forEach((el) => {
      this.productList.push({
        checked: false,
        ...el,
      });
    });
  },
  methods: {
    KeypressHandler(event) {
      console.log('event.charCode: ', event.charCode);
      return event.charCode >= 48 && event.charCode <= 57
        ? true
        : event.preventDefault();
    },
    DecrementHandler(item) {
      if (item.amount > 0) {
        item.amount--;
      }
    },
    IncrementHandler(item) {
      item.amount++;
    },
    demoHandler() {
      this.$store.commit('shoppingCart/AddProduct', {
        id: 3,
        name: 'a aaaabb b bbaaaaab bbbba aa abbbbbaaaaa bbbbbaaa aabbbbb',
        amount: 2,
        createdDate: '2022-03-17T15:43:55.653',
        description:
          'xxx xxyy yyyxxxx xy yyyy xxx xxy y yyyxx xx xyyy yyxxx xxy yyy yx x xxxyyyyyx xx xyyy yyxx xxx y yy yy xx xxxyy yyyx xxx xyy yyy',
        enabled: true,
        picture: `https://picsum.photos/200/200?image=100`,
        price: 78,
        type: 't1',
        updatedDate: '2022-03-17T15:43:55.653',
      });
    },
  },
};
</script>

<style lang="scss">
.shoppingCart {
  input[type='number']::-webkit-inner-spin-button,
  input[type='number']::-webkit-outer-spin-button {
    -webkit-appearance: none;
    margin: 0;
  }
}
</style>
