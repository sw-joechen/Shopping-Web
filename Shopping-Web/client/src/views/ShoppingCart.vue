<template>
  <div class="shoppingCart">
    <div class="wrapper max-w-[1200px] mx-auto">
      <div v-for="item in productList" :key="item.id" class="productItem flex">
        <!-- checkbox -->
        <div class="checkbox flex items-center p-2">
          <input
            class="w-5 h-5"
            type="checkbox"
            id="checkbox"
            v-model="item.checked"
            @change="CheckboxChangeHandler(item)"
          />
        </div>

        <div class="imgContainer flex justify-center p-2">
          <img class="w-16 h-16" :src="redirectImg(item.picture)" />
        </div>

        <!-- product -->
        <div class="product flex items-center p-2 max-w-[820px] flex-grow">
          <div
            :title="item.name"
            class="text-left break-all flex-grow whitespace-nowrap overflow-hidden text-ellipsis font-bold text-lg"
          >
            {{ item.name }}
          </div>
        </div>

        <!-- price -->
        <div class="price flex items-center p-2 text-lg">
          $ {{ item.price }}
        </div>

        <!-- amount -->
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

        <div class="totalPrice flex items-center p-2 text-lg">
          $ {{ item.price * item.amount }}
        </div>
        <!-- <div class="delete"></div> -->
      </div>
    </div>

    <hr class="my-2" />

    <div class="summary flex justify-end max-w-[1200px] mx-auto">
      <div class="subtotal text-red-600 text-2xl px-4 leading-10">
        $ {{ subtotal }}
      </div>
      <BtnPrimary
        label="結帳"
        @submit="CheckoutHandler"
        :disabled="!productList.length"
        theme="red"
      />
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
  watch: {
    // TODO: 看能不能做成computed
    productList: {
      deep: true,
      handler(val) {
        let readyToCheckoutList = [];
        val.forEach((el) => {
          if (el.checked) {
            readyToCheckoutList.push(el);
          }
        });
        this.subtotal = this.CalculateSubtotal(readyToCheckoutList);
      },
    },
  },
  computed: {
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
    productList() {
      return this.$store.state.shoppingCart.shoppingCart;
    },
  },
  data() {
    return {
      subtotal: 0,
    };
  },
  created() {
    this.$store.commit('shoppingCart/InitShoppingCart');
  },
  methods: {
    CheckboxChangeHandler(itemProduct) {
      let readyToCheckoutList = [];
      this.productList.forEach((el) => {
        if (el.checked) {
          readyToCheckoutList.push(el);
        }
      });
      this.subtotal = this.CalculateSubtotal(readyToCheckoutList);
      this.$store.commit('shoppingCart/EditProduct', itemProduct);
    },
    CalculateSubtotal(payload) {
      let temp = 0;
      payload.forEach((el) => {
        temp += el.price * el.amount;
      });
      return temp;
    },
    CheckoutHandler() {
      const deletedCount = this.DeleteProductItem();
      if (deletedCount === 0) {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '請勾選商品',
        });
      }
      // this.subtotal = this.CalculateSubtotal();
      this.$store.commit('eventBus/Push', {
        type: 'success',
        content: '購買成功',
      });
    },
    DeleteProductItem() {
      let prodcutIDList = [];
      this.productList.forEach((el) => {
        if (el.checked) {
          prodcutIDList.push(el.id);
        }
      });

      prodcutIDList.forEach((productID) => {
        this.$store.commit('shoppingCart/DelProduct', productID);
      });
      return prodcutIDList.length;
    },
    KeypressHandler(event) {
      console.log('event.charCode: ', event.charCode);
      return event.charCode >= 48 && event.charCode <= 57
        ? true
        : event.preventDefault();
    },
    DecrementHandler(item) {
      if (item.amount > 1) {
        item.amount--;
        this.$store.commit('shoppingCart/EditProduct', item);
      } else {
        const result = confirm('確定要刪除商品嗎');
        if (result) {
          this.$store.commit('shoppingCart/DelProduct', item.id);
        }
      }
    },
    IncrementHandler(item) {
      item.amount++;
      this.$store.commit('shoppingCart/EditProduct', item);
    },
    redirectImg(path) {
      if (process.env.NODE_ENV === 'development') return path;
      return `http://localhost:15770${path}`;
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
