using Newtonsoft.Json;

namespace Shopping_Web.Class {
    public class Result {
        /// <summary>
        /// 回應代號
        /// </summary>
        public int code;

        /// <summary>
        /// 回應訊息
        /// </summary>
        public string msg;

        /// <summary>
        /// 回應資料
        /// </summary>
        public object data = null;
        private static readonly NLog.Logger LOGGER = NLog.LogManager.GetCurrentClassLogger();

        public Result(int code, string msg) {
            this.code = code;
            this.msg = msg;
        }

        public void Set(int code, string msg, object data = null) {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
        public string Stringify() {
            string result = JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            LOGGER.Info($"result: {result}");
            return result;
        }
    }
}