<template>
  <div
    class="notificationWrapper fixed pointer-events-none w-[600px] min-h-full right-[1em] top-[1em]"
  >
    <transition-group name="fade" tag="div" class="flex flex-col">
      <NotifyView
        v-for="el in list"
        :key="el.id"
        :notify="el"
        @close="CloseHandler"
      />
    </transition-group>
  </div>
</template>

<script>
import NotifyView from './children/NotifyView.vue';
let unsubscribe = null;
export default {
  name: 'notificationWrapper',
  components: {
    NotifyView,
  },
  data: () => {
    return {
      // {
      //    id:number,
      //    type:"success" | "error",
      //    content:string,
      //    isCompleted:boolean
      // }[]
      list: [],
      uid: 0,
    };
  },
  created() {
    unsubscribe = this.$store.subscribe((mutation, state) => {
      if (mutation.type === 'eventBus/Push') {
        const notify = state.eventBus.notificationList.shift();

        this.list.push({
          id: (this.uid += 1),
          isCompleted: false,
          ...notify,
        });
      }
    });
  },
  beforeDestroy() {
    unsubscribe();
  },
  methods: {
    CloseHandler(targetID) {
      const idx = this.list.findIndex((el) => el.id === targetID);
      if (idx !== -1) {
        this.list.splice(idx, 1);
      }
    },
  },
};
</script>

<style>
.notifyView {
  transition: all 0.3s;
}
.fade-enter
/* .fade-leave-active for below version 2.1.8 */ {
  transform: translateX(300px);
}
.fade-leave-to {
  opacity: 0;
  transform: translateX(500px);
}
.fade-leave-active {
  position: absolute;
}
</style>
