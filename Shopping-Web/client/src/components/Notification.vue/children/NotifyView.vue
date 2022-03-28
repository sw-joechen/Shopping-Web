<template>
  <div
    @click="ClickHandler"
    :class="getNotifyBg"
    class="notifyView flex rounded-lg text-white pointer-events-auto ml-auto mb-4 py-[22px] px-6 min-w-[326px] min-h-[64px]"
  >
    <SuccessIcon v-show="notify.type === 'success'" />
    <ErrorIcon v-show="notify.type === 'error'" />
    <div class="txt">
      {{ notify.content }}
    </div>
  </div>
</template>

<script>
import SuccessIcon from './SuccessIcon.vue';
import ErrorIcon from './ErrorIcon.vue';
const DISPLAY_DURATION = 2000;
var timer = null;
export default {
  name: 'notifyView',
  components: {
    SuccessIcon,
    ErrorIcon,
  },
  props: {
    notify: {
      required: true,
    },
  },
  created() {
    timer = setTimeout(() => {
      this.$emit('close', this.notify.id);
    }, DISPLAY_DURATION);
  },
  computed: {
    getNotifyBg() {
      return this.notify.type === 'success' ? 'bg-green5' : 'bg-red1';
    },
  },
  methods: {
    ClickHandler() {
      clearTimeout(timer);
      this.$emit('close', this.notify.id);
    },
  },
};
</script>

<style></style>
