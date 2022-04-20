namespace Shopping_Web.Class {
    public class DemoResult<T> {
        public DemoResult(int code, string msg) {
            this.code = code;
            this.msg = msg;
        }

        /// <summary>
        /// 回應代號
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 回應訊息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 回應資料
        /// </summary>
        public T data { get; set; }

        public void Set(int code, string msg, T data = default(T)) {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
    }
}
