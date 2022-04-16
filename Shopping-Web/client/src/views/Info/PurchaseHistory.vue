<template>
  <div class="purchaseHistory flex pt-2">
    <div class="wrapper w-full">
      <div class="inputs flex mb-2">
        <DatePicker v-model="dateTime" range class="datePicker mr-2" />
        <BtnPrimary label="搜尋" @submit="SearchHandler" theme="green" />
      </div>

      <TableView :datas="purchaseHistories" />
    </div>
  </div>
</template>

<script>
import DatePicker from 'vue2-datepicker';
import BtnPrimary from '@/components/BtnPrimary.vue';
import TableView from '@/components/PurchaseHistoryTable.vue';
import 'vue2-datepicker/index.css';
import { DateTime } from 'luxon';
import { GetMemberPurchaseHistory } from '@/APIs/member';
import ErrorCodeList from '@/ErrorCodeList';
export default {
  name: 'purchaseHistory',
  components: {
    BtnPrimary,
    TableView,
    DatePicker,
  },
  data() {
    return {
      dateTime: [null, null],
      /**
       * {
       *    orderNumber:string,
       *    account:string,
       *    phone:string,
       *    address:string,
       *    createdDate:string,
       *    shoppingList:{
       *      id:number,
       *      name:string,
       *      price:number
       *      count:number
       *    }[]
       * }[]
       */
      purchaseHistories: [],
    };
  },
  computed: {
    formatStartDateTime() {
      if (!this.dateTime[0]) return null;

      const date = new Date(this.dateTime[0]);
      return DateTime.fromJSDate(date).toUTC().toISO();
    },
    formatEndDateTime() {
      if (!this.dateTime[1]) return null;

      const date = new Date(this.dateTime[1]);
      return DateTime.fromJSDate(date).toUTC().endOf('day').toISO();
    },
  },
  methods: {
    async SearchHandler() {
      const account = this.$store.state.user.account;

      const res = await GetMemberPurchaseHistory({
        account,
        startDate: this.formatStartDateTime,
        dueDate: this.formatEndDateTime,
      });
      if (res.code === 200) {
        this.purchaseHistories = res.data;
      } else {
        return this.$store.commit('eventBus/Push', {
          type: 'error',
          content: ErrorCodeList[res.code],
        });
      }
    },
  },
};
</script>

<style lang="scss">
.purchaseHistory {
  .datePicker {
    & input {
      height: 40px;
    }
  }
}
</style>
