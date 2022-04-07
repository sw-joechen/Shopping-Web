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
        <div class="product flex items-center p-2 max-w-[800px] flex-grow">
          <div
            :title="item.name"
            class="text-left break-all flex-grow overflow-hidden text-ellipsis font-bold text-lg"
          >
            {{ item.name }}
          </div>
        </div>

        <!-- price -->
        <div class="price flex items-center p-2 text-lg">
          $ {{ item.price }}
        </div>

        <!-- cartQuantity -->
        <div class="cartQuantity flex items-center p-2 w-36">
          <div class="flex h-10 w-full rounded-lg relative bg-transparent">
            <button
              @click="DecrementHandler(item)"
              class="decrement px-3 bg-gray-300 text-gray-600 hover:text-gray-700 hover:bg-gray-400 h-full rounded-l cursor-pointer outline-none"
            >
              <span class="m-auto text-2xl font-thin">−</span>
            </button>
            <input
              v-model="item.cartQuantity"
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
          $ {{ item.price * item.cartQuantity }}
        </div>

        <div class="delete flex items-center">
          <div
            class="txt whitespace-nowrap cursor-pointer hover:text-green1"
            @click="DelProductHandler(item.id)"
          >
            刪除
          </div>
        </div>
      </div>
    </div>

    <hr class="my-2" />

    <div
      class="summary flex max-w-[1200px] mx-auto"
      :class="productList.length ? 'justify-between' : 'justify-end'"
    >
      <!-- selectAll -->
      <div class="selectAll flex items-center m-2" v-if="productList.length">
        <input
          class="w-5 h-5 mr-4"
          type="checkbox"
          id="selectAll"
          v-model="selectAll"
          @change="SelectAllChangeHandler"
        />
        <label for="selectAll">全選</label>
      </div>
      <div class="flex">
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
  </div>
</template>

<script>
import BtnPrimary from '@/components/BtnPrimary.vue';
export default {
  name: 'shoppingCart',
  components: {
    BtnPrimary,
  },
  data() {
    return {
      selectAll: false,
    };
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
    subtotal() {
      let test = 0;
      this.productList.forEach((prod) => {
        if (prod.checked) {
          test += prod.price * prod.cartQuantity;
        }
      });
      return test;
    },
  },
  created() {
    this.$store.dispatch('shoppingCart/InitShoppingCart');
  },
  methods: {
    SelectAllChangeHandler() {
      this.productList.forEach((prod) => {
        this.$store.commit('shoppingCart/EditProduct', {
          ...prod,
          checked: this.selectAll,
        });
      });
    },
    CheckboxChangeHandler() {
      let readyToCheckoutList = [];
      this.productList.forEach((el) => {
        if (el.checked) {
          readyToCheckoutList.push(el);
        }
      });
    },
    CheckoutHandler() {
      const deletedCount = this.BatchDeleteProductItem();
      if (deletedCount === 0) {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '請勾選商品',
        });
      }

      this.$store.commit('eventBus/Push', {
        type: 'success',
        content: '購買成功',
      });
    },
    BatchDeleteProductItem() {
      let productIDList = [];
      this.productList.forEach((el) => {
        if (el.checked) {
          productIDList.push(el.id);
        }
      });

      productIDList.forEach((productID) => {
        this.$store.commit('shoppingCart/DelProduct', productID);
      });
      return productIDList.length;
    },
    DelProductHandler(targetID) {
      this.$store.commit('shoppingCart/DelProduct', targetID);
    },
    KeypressHandler(event) {
      console.log('event.charCode: ', event.charCode);
      return event.charCode >= 48 && event.charCode <= 57
        ? true
        : event.preventDefault();
    },
    DecrementHandler(item) {
      if (item.cartQuantity > 1) {
        item.cartQuantity--;
        this.$store.commit('shoppingCart/EditProduct', item);
      } else {
        const result = confirm('確定要刪除商品嗎');
        if (result) {
          this.$store.commit('shoppingCart/DelProduct', item.id);
        }
      }
    },
    IncrementHandler(item) {
      if (item.cartQuantity < item.amount) {
        item.cartQuantity++;
        this.$store.commit('shoppingCart/EditProduct', item);
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '已達購買上限',
        });
      }
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
