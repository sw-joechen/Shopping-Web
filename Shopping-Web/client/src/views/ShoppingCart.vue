<template>
  <div class="shoppingCart">
    <div class="wrapper max-w-[1200px] mx-auto mb-2">
      <!-- 表頭 -->
      <div class="header flex border-b border-gray2">
        <!-- selectAll -->
        <div
          class="selectAll flex items-center m-2 w-[788px]"
          v-if="productList.length"
        >
          <input
            class="w-5 h-5 mr-4 accent-green2"
            type="checkbox"
            id="selectAll"
            v-model="selectAll"
            @change="SelectAllChangeHandler"
          />
          <label for="selectAll">商品</label>
        </div>
        <div class="price w-[110px] leading-10">單價</div>
        <div class="amount w-36 leading-10">數量</div>
        <div class="totalPrice w-[110px] leading-10">總計</div>
        <div class="operation leading-10">操作</div>
      </div>

      <!-- 內容 -->
      <div
        v-for="item in productList"
        :key="item.id"
        class="productItem flex border-b border-gray2"
      >
        <!-- checkbox -->
        <div class="checkbox flex items-center p-2">
          <input
            :class="{ 'opacity-50': !item.enabled }"
            class="w-5 h-5 accent-green2"
            type="checkbox"
            id="checkbox"
            v-model="item.checked"
            @change="CheckboxChangeHandler(item)"
          />
        </div>

        <div class="imgContainer flex justify-center py-2">
          <img
            class="w-16 h-16"
            :src="redirectImg(item.picture)"
            :class="{ 'opacity-50': !item.enabled }"
          />
        </div>

        <!-- product -->
        <div
          class="product flex items-center p-2 w-[700px] flex-grow"
          :class="{ 'opacity-50': !item.enabled }"
        >
          <div
            :title="item.name"
            class="text-left break-all flex-grow overflow-hidden text-ellipsis font-bold text-lg"
            :class="{ 'line-through': !item.enabled }"
          >
            {{ item.name }}
          </div>
        </div>

        <!-- price -->
        <div
          class="price flex justify-end items-center p-2 text-lg w-[110px]"
          :class="{ 'line-through opacity-50': !item.enabled }"
        >
          $ {{ item.price }}
        </div>

        <!-- cartQuantity -->
        <div
          class="cartQuantity flex items-center p-2 w-36"
          :class="{ 'pointer-events-none opacity-50': !item.enabled }"
        >
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

        <div
          :class="{ 'line-through opacity-50': !item.enabled }"
          class="totalPrice flex justify-center items-center p-2 text-lg w-[110px]"
        >
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

    <div
      class="summary flex max-w-[1200px] mx-auto"
      :class="productList.length ? 'justify-between' : 'justify-end'"
    >
      <div class="flex">
        <!-- selectAll -->
        <div class="selectAll flex items-center m-2" v-if="productList.length">
          <input
            class="w-5 h-5 mr-4 accent-green2"
            type="checkbox"
            id="selectAll"
            v-model="selectAll"
            @change="SelectAllChangeHandler"
          />
          <label for="selectAll">全選</label>
        </div>
        <div class="batchDel mx-3">
          <BtnPrimary
            label="刪除"
            @submit="TotalDelHandler"
            :disabled="!productList.length"
            theme="red"
          />
        </div>
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
        if (prod.checked && prod.enabled) {
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
    resetSelectAll() {
      this.selectAll = false;
    },
    TotalDelHandler() {
      const result = confirm('確定要刪除勾選的商品嗎');
      if (result) {
        const ready2BeDeletedList = [];
        this.productList.forEach((el) => {
          if (el.checked) {
            ready2BeDeletedList.push(el.id);
          }
        });

        this.BatchDeleteProductItem(ready2BeDeletedList);
        this.resetSelectAll();
      }
    },
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
      const ready2BeDeletedList = [];
      this.productList.forEach((el) => {
        if (el.checked && el.enabled) {
          ready2BeDeletedList.push(el.id);
        }
      });

      this.BatchDeleteProductItem(ready2BeDeletedList);
      if (ready2BeDeletedList.length === 0) {
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
    BatchDeleteProductItem(list) {
      list.forEach((productID) => {
        this.$store.commit('shoppingCart/DelProduct', productID);
      });
    },
    DelProductHandler(targetID) {
      const result = confirm('確定要刪除商品嗎');
      if (result) this.$store.commit('shoppingCart/DelProduct', targetID);
    },
    KeypressHandler(event) {
      console.log('event.charCode: ', event.charCode);
      return event.charCode >= 48 && event.charCode <= 57
        ? true
        : event.preventDefault();
    },
    DecrementHandler(item) {
      if (!item.enabled) return;
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
      if (!item.enabled) return;
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
