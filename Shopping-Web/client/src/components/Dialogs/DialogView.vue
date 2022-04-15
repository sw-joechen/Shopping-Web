<template>
  <div class="dialogView fixed inset-0 pointer-events-none">
    <!-- overlay -->
    <div
      class="overlay fixed top-0 left-0 w-full h-full bg-black1 pointer-events-auto"
      @click="ToggleHandler"
    ></div>

    <!-- modal -->
    <div
      class="dialog_inner flex justify-center items-center p-6 pointer-events-none fixed inset-0"
    >
      <div
        :class="maxWidth"
        class="dialog_card relative rounded-[4px] bg-white p-4 pointer-events-auto"
      >
        <div class="mb-5 text-2xl">{{ title }}</div>
        <div class="form">
          <slot></slot>
        </div>
        <div class="bottom flex justify-center p-3">
          <BtnSubmit label="確定" @submit="SubmitHandler" />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import BtnSubmit from '../BtnPrimary.vue';
export default {
  name: 'DialogView',
  components: {
    BtnSubmit,
  },
  props: {
    isShowDialog: {
      required: true,
      type: Boolean,
    },
    title: {
      required: true,
      type: String,
    },
    maxWidth: {
      required: false,
      type: String,
      default: 'max-w-[560px]',
    },
  },
  methods: {
    ToggleHandler() {
      this.$emit('toggle', !this.isShowDialog);
    },
    SubmitHandler() {
      this.$emit('submit');
    },
  },
};
</script>

<style lang="scss">
.dialogView {
  .dialog_inner {
    .dialog_card {
      max-height: calc(100vh - 48px);
      box-shadow: 0 2px 4px -1px #0003, 0 4px 5px #00000024,
        0 1px 10px #0000001f;
    }
  }
}
</style>
