<template>
  <div class="purchaseHistoryTable">
    <table class="w-full pt-5">
      <thead>
        <tr
          class="text-left border-t border-b-2 border-grey2 p-2 bg-green7 text-white text-xl;"
        >
          <th
            class="py-3 px-1 min-w-[120px] text-center"
            scope="col"
            v-for="col in columns"
            :key="col"
          >
            {{ $t(`purchaseHistoryTable.${col}`) }}
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="(item, index) in datas"
          :key="index"
          class="even:bg-gray-300 hover:bg-gray-400 even:border-b odd:bg-white border-gray-400 p-2"
        >
          <td class="orderNumber bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.orderNumber }}
              </span>
            </div>
          </td>

          <td class="account bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.account }}
              </span>
            </div>
          </td>

          <td class="address bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.address }}
              </span>
            </div>
          </td>

          <td class="phone bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ item.phone }}
              </span>
            </div>
          </td>

          <td class="shoppingList bodyTd">
            <div class="max-w-xs text-left flex flex-col">
              <div
                :title="`${idx + 1}. ${prod.name} 數量: ${prod.count}`"
                class="item"
                v-for="(prod, idx) in item.shoppingList"
                :key="prod.id"
              >
                <span class="break-all">
                  <strong class="text-green4">{{ idx + 1 }}.</strong>
                  {{ prod.name }}, 數量: {{ prod.count }}, 單價:
                  {{ prod.price }}
                </span>
              </div>
            </div>
          </td>

          <td class="orderPrice bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ CalculateOrderPrice(item.shoppingList) }}
              </span>
            </div>
          </td>

          <td class="createdDate bodyTd">
            <div class="max-w-xs text-center">
              <span class="break-all">
                {{ DateFormatter(item.createdDate) }}
              </span>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import { DateTime } from 'luxon';
export default {
  name: 'purchaseHistoryTable',
  props: {
    datas: {
      required: true,
      type: Array,
    },
  },
  data: () => {
    return {
      columns: [
        'orderNumber',
        'account',
        'address',
        'phone',
        'shoppingList',
        'orderPrice',
        'createdDate',
      ],
    };
  },
  methods: {
    // 將日期格式化
    DateFormatter(inputDate) {
      const date = new Date(inputDate);
      const dt = DateTime.fromJSDate(date);
      return dt.toFormat('yyyy/MM/dd HH:mm:ss');
    },

    CalculateOrderPrice(shoppingList) {
      let totalPrice = 0;
      shoppingList.forEach((product) => {
        totalPrice += product.price * product.count;
      });
      return totalPrice;
    },
  },
};
</script>

<style lang="scss">
.purchaseHistoryTable {
  .bodyTd {
    @apply py-3 px-1 text-left;
  }
}
</style>
