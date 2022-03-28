<template>
  <div class="btnPrimary">
    <button
      @click="ClickHandler"
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
  },
  methods: {
    ClickHandler() {
      this.$emit('submit');
    },
  },
};
</script>

<style></style>
