<template>
  <div class="btnPrimary">
    <button
      @click="ClickHandler"
      :class="[getTheme, { 'cursor-not-allowed opacity-80': disabled }, px]"
      class="text-white font-bold py-2 rounded"
      :disabled="disabled"
    >
      {{ label }}
    </button>
  </div>
</template>

<script>
export default {
  props: {
    label: {
      required: true,
      type: String,
    },
    width: {
      required: false,
      default: '',
    },
    theme: {
      required: false,
      type: String,
    },
    disabled: {
      required: false,
      type: Boolean,
    },
    px: {
      required: false,
      type: String,
      default: 'px-4',
    },
  },
  computed: {
    getTheme() {
      switch (this.theme) {
        case 'red': {
          return 'bg-red-500 hover:bg-red-700 focus:ring-4 focus:ring-red-300';
        }
        case 'green': {
          return 'bg-green-500 hover:bg-green-700 focus:ring-4 focus:ring-green-300';
        }
        case 'white': {
          return 'bg-white hover:bg-gray-100 text-gray-800  shadow font-semibold';
        }
        default: {
          return 'bg-blue-500 hover:bg-blue-700 focus:ring-4 focus:ring-blue-300';
        }
      }
    },
  },
  methods: {
    ClickHandler() {
      this.$emit('submit');
    },
  },
};
</script>

<style></style>
