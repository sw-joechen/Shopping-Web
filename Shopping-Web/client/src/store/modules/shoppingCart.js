const state = () => ({
  shoppingCart: [],
});

const getters = {};

const actions = {};

const mutations = {
  AddProduct(state, payload) {
    state.shoppingCart.push(payload);
    this.commit('shoppingCart/SetLocalStorage');
  },
  EditProduct(state, payload) {
    const targetIndex = state.shoppingCart.findIndex(
      (el) => el.id === payload.id
    );
    if (targetIndex !== -1) {
      state.shoppingCart.splice(targetIndex, 1, payload);
    }
  },
  DelProduct(state, productID) {
    const targetIndex = state.shoppingCart.findIndex(
      (el) => el.id === productID
    );
    if (targetIndex !== -1) {
      state.shoppingCart.splice(targetIndex, 1);
    }
  },
  InitShoppingCart(state) {
    const res = localStorage.getItem('client_shopping_cart');
    console.log(res);
    if (res) {
      state.shoppingCart = JSON.parse(res);
    }
  },
  SetLocalStorage(state) {
    localStorage.setItem(
      'client_shopping_cart',
      JSON.stringify(state.shoppingCart)
    );
  },
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations,
};
