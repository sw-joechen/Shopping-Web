using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Shopping_Web.Class;

namespace Shopping_Web.Controllers {
    public class ProductController : ApiController {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 取得商品清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getProductsList")]
        public string GetProductsList() {
            var httpRequest = HttpContext.Current.Request;
            Result result = new Result(100, "缺少參數");
            List<Product> productList = new List<Product> { };

            try {
                string paramID = httpRequest.Params["id"];
                string name = httpRequest.Params["name"] ?? null;
                string type = httpRequest.Params["type"] ?? null;
                string paramEnabled = httpRequest.Params["enabled"];

                int enabled = paramEnabled != null ? Convert.ToInt32(Convert.ToBoolean(paramEnabled)) : -1;
                int id = paramID != null ? Convert.ToInt32(paramID) : -1;

                Logger.Info($"API: getProductsList, id: {id}, name: {name}, type: {type}, enabled: {enabled}");

                using (SqlConnection conn = new SqlConnection(connectString)) {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_getProductList", conn)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (id == -1)
                            cmd.Parameters.AddWithValue("@id", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@id", id);

                        if (name == null)
                            cmd.Parameters.AddWithValue("@name", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@name", name);

                        if (type == null)
                            cmd.Parameters.AddWithValue("@type", DBNull.Value);

                        else
                            cmd.Parameters.AddWithValue("@type", type);

                        if (enabled == -1)
                            cmd.Parameters.AddWithValue("@enabled", DBNull.Value);

                        else
                            cmd.Parameters.AddWithValue("@enabled", enabled);

                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows) {
                            while (r.Read()) {
                                productList.Add(new Product {
                                    id = Convert.ToInt32(r["f_id"]),
                                    name = r["f_name"].ToString(),
                                    description = r["f_description"].ToString(),
                                    price = Convert.ToInt32(r["f_price"]),
                                    picture = $"/Uploads{r["f_picture"]}",
                                    amount = Convert.ToInt32(r["f_amount"]),
                                    type = r["f_type"].ToString(),
                                    enabled = Convert.ToBoolean(Convert.ToInt16(r["f_enabled"])),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
                                });
                            }
                            result.Set(200, "success", productList);
                        }
                        else {
                            result.Set(112, "找不到該商品");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(ex);
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }

        /// <summary>
        /// 批次搜尋商品清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getProductListByID")]
        public DemoResult<List<Product>> GetProductListByID([FromBody] GetProductListByIDPayload payload) {
            var result = new DemoResult<List<Product>>(100, "缺少參數");
            List<Product> productList = new List<Product> { };

            if (payload == null || payload.productIdList == null) {
                return result;
            }

            // 帶空陣列進來直接回空陣列結束
            if (payload.productIdList.Count == 0) {
                result.Set(200, "success", productList);
                return result;
            }

            Logger.Info($"API: getProductListByID, payload: {JsonConvert.SerializeObject(payload)}");

            try {
                List<int> productIDList = payload.productIdList;

                // List轉成datatable
                DataTable productDt = new DataTable();
                productDt.Columns.Add("ID", typeof(int));
                for (int i = 0; i < productIDList.Count; i++) {
                    productDt.Rows.Add(productIDList[i]);
                }

                using (SqlConnection conn = new SqlConnection(connectString)) {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_getProductListByID", conn)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ids", productDt);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows) {
                            while (r.Read()) {
                                productList.Add(new Product {
                                    id = Convert.ToInt32(r["f_id"]),
                                    name = r["f_name"].ToString(),
                                    description = r["f_description"].ToString(),
                                    price = Convert.ToInt32(r["f_price"]),
                                    picture = $"/Uploads{r["f_picture"]}",
                                    amount = Convert.ToInt32(r["f_amount"]),
                                    type = r["f_type"].ToString(),
                                    enabled = Convert.ToBoolean(Convert.ToInt16(r["f_enabled"])),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
                                });
                            }
                            result.Set(200, "success", productList);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(ex);
                result.Set(101, "網路錯誤");
            }
            return result;
        }
    }
}
