namespace Shopping_Web.Class {
    public class RejectedProduct {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 拒絕狀況 0:狀態錯誤, 1:數量錯誤, 2:id在清單中匹配不到, 3:價格有異動
        /// </summary>
        public int rejectCondition { get; set; }
    }
}
