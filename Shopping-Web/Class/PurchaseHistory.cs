using System.Collections.Generic;

namespace Shopping_Web.Class {
    public class PurchaseHistory {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public string orderNumber { get; set; }

        /// <summary>
        /// 購買人帳號
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 購買人電話
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 購買人地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string createdDate { get; set; }

        /// <summary>
        /// 購買商品清單
        /// </summary>
        public List<HistoryPurchasedItem> shoppingList { get; set; }
    }
}
