import { GetProductList } from '@/APIs/product';

const state = () => ({
  // {
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
  // cartQuantity:number
  // checked:boolean
  // }
  shoppingCart: [],
});

const getters = {};

const actions = {
  async InitShoppingCart(context) {
    let lsList = [];
    // 從localStorage取出暫存清單
    if (localStorage.getItem('client_shopping_cart'))
      lsList = JSON.parse(localStorage.getItem('client_shopping_cart'));

    // TODO: 應該要改呼叫批次搜尋id的接口
    let productList = [];
    const res = await GetProductList();
    if (res.code === 200) {
      productList = res.data;
    }

    // 初始化購物車
    context.state.shoppingCart.splice(0, context.state.shoppingCart.length);
    lsList.forEach((cartItem) => {
      return productList.some((product) => {
        if (cartItem.id === product.id) {
          context.state.shoppingCart.push({
            ...product,
            cartQuantity: cartItem.cartQuantity,
            checked: false,
          });
          return true;
        }
      });
    });
  },
};

const mutations = {
  AddProduct(state, payload) {
    // 先確認有沒有重複的商品, ＋1後的數量有沒有超過上限, 都符合的話數量+1
    const idx = state.shoppingCart.findIndex((item) => item.id === payload.id);
    if (idx !== -1) {
      const cartQuantity = state.shoppingCart[idx].cartQuantity;
      if (cartQuantity < state.shoppingCart[idx].amount) {
        state.shoppingCart.splice(idx, 1, {
          ...payload,
          cartQuantity: cartQuantity + 1,
        });

        this.commit('eventBus/Push', {
          type: 'success',
          content: '加入購物車成功',
        });
      } else {
        alert('已達數量上限');
      }
    } else {
      // 新增商品
      state.shoppingCart.push({
        ...payload,
        cartQuantity: 1,
      });

      this.commit('eventBus/Push', {
        type: 'success',
        content: '加入購物車成功',
      });
    }

    // 同步localStorage資料
    this.commit('shoppingCart/SetLocalStorage');
  },
  EditProduct(state, payload) {
    const targetIndex = state.shoppingCart.findIndex(
      (el) => el.id === payload.id
    );
    if (targetIndex !== -1) {
      state.shoppingCart.splice(targetIndex, 1, payload);

      // 同步localStorage資料
      this.commit('shoppingCart/SetLocalStorage');
    }
  },
  DelProduct(state, productID) {
    const targetIndex = state.shoppingCart.findIndex(
      (el) => el.id === productID
    );
    if (targetIndex !== -1) {
      state.shoppingCart.splice(targetIndex, 1);

      // 同步localStorage資料
      this.commit('shoppingCart/SetLocalStorage');
    }
  },
  SetLocalStorage(state) {
    // localStorage只存id, 數量
    // id
    // cartQuantity
    // 每一次的新刪修都會觸發同步LS, 這時候會清除已經記在LS但是被刪掉的商品
    const ls = [];
    state.shoppingCart.forEach((product) => {
      ls.push({
        id: product.id,
        cartQuantity: product.cartQuantity,
      });
    });
    localStorage.setItem('client_shopping_cart', JSON.stringify(ls));
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
