<template>
  <div class="layoutView p-2 bg-black4">
    <NavBar />
    <router-view />
  </div>
</template>

<script>
import NavBar from '@/components/NavBar.vue';
export default {
  name: 'layoutView',
  components: {
    NavBar,
  },
};
</script>
