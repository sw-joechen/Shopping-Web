using Newtonsoft.Json;

namespace Shopping_Web.Class {
    public class Result {
        public int code;
        public string msg;
        public object data = null;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

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
            Logger.Info($"result: {result}");
            return result;
        }
    }
}