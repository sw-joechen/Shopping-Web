<template>
  <div class="btnPrimary" :class="width">
    <button
      @click="ClickHandler"
      :class="width"
      class="bg-green1 hover:bg-green2 focus:ring-2 ring-green4 text-white font-bold py-2 px-4 rounded"
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
  },
  methods: {
    ClickHandler() {
      this.$emit('submit');
    },
  },
};
</script>

<style></style>
