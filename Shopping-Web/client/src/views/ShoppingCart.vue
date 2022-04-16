<template>
  <div class="shoppingCart">
    <div class="wrapper max-w-[1200px] mx-auto mb-2">
      <!-- 表頭 -->
      <!-- TODO: 改用table -->
      <div class="header flex border-b border-gray2">
        <!-- selectAll -->
        <div
          class="selectAll flex items-center m-2 w-[788px]"
          v-if="productList.length"
        >
          <input
            id="selectAll"
            type="checkbox"
            class="hidden peer"
            v-model="selectAll"
            :checked="selectAll"
            @change="SelectAllChangeHandler"
          />
          <label
            for="selectAll"
            class="flex items-center cursor-pointer select-none"
          >
            <span
              class="w-5 h-5 mr-4 inline-block rounded-[4px] border border-green3 transition-all duration-200 bg-white"
              :class="{
                'bg-green6 shadow-radioBtn1': selectAll,
              }"
            />
            商品
          </label>
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
            :id="`checkox-${item.id}`"
            type="checkbox"
            class="hidden peer"
            v-model="item.checked"
            :checked="item.checked"
            @change="CheckboxChangeHandler(item)"
          />
          <label
            :for="`checkox-${item.id}`"
            class="flex items-center cursor-pointer select-none"
          >
            <span
              class="w-5 h-5 inline-block rounded-[4px] border border-green3 transition-all duration-200 bg-white"
              :class="[
                { 'opacity-50': !item.enabled || item.amount === 0 },
                { 'bg-green6 shadow-radioBtn1': item.checked },
              ]"
            />
          </label>
        </div>

        <div class="imgContainer flex justify-center py-2">
          <img
            class="w-16 h-16"
            :src="redirectImg(item.picture)"
            :class="{ 'opacity-50': !item.enabled || item.amount === 0 }"
          />
        </div>

        <!-- product -->
        <div
          class="product flex items-center p-2 w-[700px] flex-grow"
          :class="{ 'opacity-50': !item.enabled || item.amount === 0 }"
        >
          <div
            :title="item.name"
            class="text-left break-all flex-grow overflow-hidden text-ellipsis font-bold text-lg"
            :class="{ 'line-through': !item.enabled || item.amount === 0 }"
          >
            {{ item.name }}
          </div>
        </div>

        <div
          class="failHint flex items-center text-red-600"
          v-if="IsShowProductAlert(item.id)"
        >
          <ErrorIcon
            :title="GetProductAlertTitle(item.id)"
            @click.native="CloseProductAlertHandler(item.id)"
          />
        </div>

        <!-- price -->
        <div
          class="price flex justify-end items-center p-2 text-lg w-[110px]"
          :class="{
            'line-through opacity-50': !item.enabled || item.amount === 0,
          }"
        >
          $ {{ item.price }}
        </div>

        <!-- cartQuantity -->
        <div class="cartQuantity flex items-center p-2 w-36 relative">
          <div
            class="flex h-10 w-full rounded-lg relative bg-transparent"
            :class="{
              'pointer-events-none opacity-50':
                !item.enabled || item.amount === 0,
            }"
          >
            <button
              @click="DecrementHandler(item)"
              class="decrement px-3 bg-gray-300 text-gray-600 hover:text-gray-700 hover:bg-gray-400 h-full rounded-l cursor-pointer outline-none"
            >
              <span class="m-auto text-2xl font-thin">−</span>
            </button>
            <input
              v-model.number="item.cartQuantity"
              type="number"
              class="w-full outline-none focus:outline-none text-center bg-gray-300 font-semibold text-md hover:text-black focus:text-black flex items-center text-gray-700"
              @keypress="KeypressHandler"
              @change="CartQuantityChangeHandler(item)"
            />
            <button
              @click="IncrementHandler(item)"
              class="increment px-3 bg-gray-300 text-gray-600 hover:text-gray-700 hover:bg-gray-400 h-full rounded-r cursor-pointer"
            >
              <span class="m-auto text-2xl font-thin">+</span>
            </button>
          </div>
          <div
            class="soldout absolute top-1/2 left-1/2 -translate-x-1/2 -translate-y-1/2 text-red-500 text-2xl"
            v-if="item.amount === 0"
          >
            已售完
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

    <div class="summary flex max-w-[1200px] mx-auto justify-between">
      <div class="flex">
        <!-- selectAll -->
        <div class="selectAll flex items-center m-2" v-if="productList.length">
          <input
            id="selectAll"
            type="checkbox"
            class="hidden peer"
            v-model="selectAll"
            :checked="selectAll"
          />
          <label
            for="selectAll"
            class="flex items-center cursor-pointer select-none"
          >
            <span
              class="w-5 h-5 mr-4 inline-block rounded-[4px] border border-green3 transition-all duration-200 bg-white"
              :class="{
                'bg-green6 shadow-radioBtn1': selectAll,
              }"
            />
            全選
          </label>
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
          @submit="ReadyToCheckoutHandler"
          :disabled="!productList.length"
          theme="red"
        />
      </div>
    </div>

    <!-- 結帳對話窗 -->
    <FormDialog
      title="確認訂單"
      v-if="isShowConfirmDialog"
      :isShowDialog="isShowConfirmDialog"
      @toggle="ToggleConfirmDialogHandler"
      @submit="SubmitCheckoutHandler"
      maxWidth="max-w-[960px]"
    >
      <table class="w-full pt-5">
        <thead>
          <tr
            class="text-left border-t border-b-2 border-grey2 p-2 bg-green7 text-white text-xl;"
          >
            <th
              class="py-3 px-1 min-w-[120px] text-center"
              scope="col"
              v-for="col in columns"
              :key="col"
            >
              {{ col }}
            </th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="item in finalConfirmProductList"
            :key="item.id"
            class="even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2"
          >
            <td class="name bodyTd">
              <div class="max-w-xs text-center">
                <span class="break-all">
                  {{ item.name }}
                </span>
              </div>
            </td>

            <td class="price bodyTd">
              <div class="max-w-xs text-center">
                <span class="break-all"> $ {{ item.price }} </span>
              </div>
            </td>

            <td class="cartQuantity bodyTd">
              <div class="max-w-xs text-center">
                <span class="break-all">
                  {{ item.cartQuantity }}
                </span>
              </div>
            </td>

            <td class="totalPrice bodyTd">
              <div class="max-w-xs text-center">
                <span class="break-all">
                  $ {{ item.price * item.cartQuantity }}
                </span>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
      <div class="flex">
        <div class="subtotal text-red-600 text-2xl px-4 leading-10">
          $ {{ subtotal }}
        </div>
      </div>
    </FormDialog>
  </div>
</template>

<script>
import BtnPrimary from '@/components/BtnPrimary.vue';
import FormDialog from '@/components/Dialogs/DialogView.vue';
import { Purchase, GetMemberPersonalInfo } from '@/APIs/member';
import ErrorCodeList from '@/ErrorCodeList';
import ErrorIcon from '@/components/Notification.vue/children/ErrorIcon.vue';

// rejectCondition=> 0: 狀態錯誤, 1:數量錯誤, 2:id在清單中匹配不到, 3:價格有異動
const ERejectCondition = {
  status: 0,
  quantity: 1,
  deleted: 2,
  priceChanged: 3,
};
export default {
  name: 'shoppingCart',
  components: {
    BtnPrimary,
    FormDialog,
    ErrorIcon,
  },
  data() {
    return {
      selectAll: false,
      isShowConfirmDialog: false,
      columns: ['名稱', '單價', '數量', '總計'],
      rejectedProductList: [],
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
    checkedAndEnabledProductList() {
      return this.productList.reduce((prev, current) => {
        if (current.checked && current.enabled) {
          return [...prev, current];
        }
        return prev;
      }, []);
    },
    finalConfirmProductList() {
      return this.checkedAndEnabledProductList.reduce((prev, current) => {
        if (current.amount > 0) {
          return [...prev, current];
        }
        return prev;
      }, []);
    },
    subtotal() {
      let result = 0;
      this.checkedAndEnabledProductList.forEach((prod) => {
        if (prod.amount > 0) {
          result += prod.price * prod.cartQuantity;
        }
      });
      return result;
    },
  },
  created() {
    this.$store.dispatch('shoppingCart/InitShoppingCart');
  },
  methods: {
    CloseProductAlertHandler(payloadID) {
      const idx = this.rejectedProductList.findIndex(
        (el) => el.id === payloadID
      );
      if (idx !== -1) {
        this.rejectedProductList.splice(idx, 1);
      }
    },
    GetProductAlertTitle(payloadID) {
      const result = this.rejectedProductList.find((el) => el.id === payloadID);
      if (result) {
        switch (result.rejectCondition) {
          case ERejectCondition.status:
            return '商品已被禁用';
          case ERejectCondition.quantity:
            return '請確認商品數量';
          case ERejectCondition.deleted:
            return '商品已被刪除';
          case ERejectCondition.priceChanged:
            return '商品價格異動，請確認商品';
          default:
            break;
        }
      }
    },
    IsShowProductAlert(payloadID) {
      const result = this.rejectedProductList.find((el) => el.id === payloadID);
      if (result) {
        return true;
      }
      return false;
    },
    ReadyToCheckoutHandler() {
      const ready2BeDeletedList = [];
      this.checkedAndEnabledProductList.forEach((prod) => {
        if (prod.amount > 0) {
          ready2BeDeletedList.push(prod.id);
        }
      });

      if (ready2BeDeletedList.length === 0) {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '請勾選商品',
        });
      }
      this.ToggleConfirmDialogHandler();
    },
    async SubmitCheckoutHandler() {
      const userBalance = await this.GetMemberBalance();
      if (userBalance >= this.subtotal) {
        await this.CheckoutHandler();
        this.ToggleConfirmDialogHandler();
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '餘額不足',
        });
      }
    },
    async GetMemberBalance() {
      const fd = new FormData();
      fd.append('account', this.$store.state.user.account);

      const res = await GetMemberPersonalInfo(fd);
      if (res.code === 200 && res.data.length) {
        return res.data[0].balance;
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '網路錯誤',
        });
        return null;
      }
    },
    ToggleConfirmDialogHandler() {
      this.isShowConfirmDialog = !this.isShowConfirmDialog;
    },
    CartQuantityChangeHandler(product) {
      if (product.cartQuantity > product.amount) {
        alert('已超過可購買數量');
        product.cartQuantity = product.amount;
      } else if (product.cartQuantity <= 0) {
        alert('最少選購數量為1');
        product.cartQuantity = 1;
      }
      this.$store.commit('shoppingCart/EditProduct', product);
    },
    resetSelectAll() {
      this.selectAll = false;
    },
    TotalDelHandler() {
      const ready2BeDeletedList = [];
      this.productList.forEach((el) => {
        if (el.checked) {
          ready2BeDeletedList.push(el.id);
        }
      });

      if (!ready2BeDeletedList.length) {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '請勾選商品',
        });
      }

      const result = confirm('確定要刪除勾選的商品嗎');
      if (result) {
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
      // TODO: try try js.every
      // 只要有項目沒有被勾選, 就重置全選checkbox
      this.selectAll = !this.productList.some((el) => {
        if (!el.checked) return true;
      });
    },
    async CheckoutHandler() {
      const ready2BeDeletedList = [];
      this.checkedAndEnabledProductList.forEach((prod) => {
        ready2BeDeletedList.push(prod.id);
      });

      // 整理要送出結帳的商品清單
      let shoppingList = [];
      this.checkedAndEnabledProductList.forEach((el) => {
        shoppingList.push({
          id: el.id,
          count: el.cartQuantity,
          price: el.price,
        });
      });

      const res = await Purchase({
        account: this.$store.state.user.account,
        shoppingList,
      });

      if (res.code === 200) {
        this.BatchDeleteProductItem(ready2BeDeletedList);

        this.$store.commit('eventBus/Push', {
          type: 'success',
          content: '購買成功',
        });
      } else if (res.code === 119) {
        // 存取被拒絕的商品
        this.rejectedProductList = res.data;

        this.$store.dispatch('shoppingCart/InitShoppingCart');
        this.resetSelectAll();
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      } else {
        this.$store.commit('eventBus/Push', {
          type: 'error',
          content: '網路錯誤',
        });
      }
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
      return `http://localhost:8080${path}`;
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
