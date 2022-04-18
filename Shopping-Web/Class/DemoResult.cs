namespace Shopping_Web.Class {
    public class DemoResult<T> {
        public DemoResult(int code, string msg) {
            this.code = code;
            this.msg = msg;
        }

        public int code { get; set; }
        public string msg { get; set; }
        public T data { get; set; }

        public void Set(int code, string msg, T data = default(T)) {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
    }
}
