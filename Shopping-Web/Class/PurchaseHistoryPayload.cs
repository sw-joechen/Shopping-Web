namespace Shopping_Web.Class {
    public class PurchaseHistoryPayload {
        /// <summary>
        /// 帳號
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public string dueDate { get; set; }
    }
}
