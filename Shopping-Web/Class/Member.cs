namespace Shopping_Web.Class {
    public class Member {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 會員帳號
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 會員地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 會員電話
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 會員性別
        /// </summary>
        public int gender { get; set; }

        /// <summary>
        /// 電子信箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 啟用
        /// </summary>
        public bool enabled { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string createdDate { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string updatedDate { get; set; }

        /// <summary>
        /// 會員餘額
        /// </summary>
        public double balance { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string pwd { get; set; }
    }
}