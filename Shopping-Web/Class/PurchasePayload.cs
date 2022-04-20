using System.Collections.Generic;

namespace Shopping_Web.Class {
    public class PurchasePayload {
        /// <summary>
        /// 帳號
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 購買商品清單
        /// </summary>
        public List<PurchaseItem> shoppingList { get; set; }
    }
}
