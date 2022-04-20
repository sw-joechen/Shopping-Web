namespace Shopping_Web.Class {
    public class Product {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 啟用
        /// </summary>
        public bool enabled { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        public int price { get; set; }

        /// <summary>
        /// 商品照片
        /// </summary>
        public string picture { get; set; }

        /// <summary>
        /// 商品庫存數量
        /// </summary>
        public int amount { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string createdDate { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string updatedDate { get; set; }
    }
}