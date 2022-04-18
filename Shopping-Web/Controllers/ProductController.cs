using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Shopping_Web.Class;

namespace Shopping_Web.Controllers
{
    public class ProductController : ApiController
    {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        /// <summary>
        /// 取得商品清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getProductsList")]
        public string GetProductsList()
        {
            var httpRequest = HttpContext.Current.Request;
            Result result = new Result(100, "缺少參數");
            List<Product> productList = new List<Product> { };

            try
            {
                string paramID = httpRequest.Params["id"];
                string name = httpRequest.Params["name"] ?? null;
                string type = httpRequest.Params["type"] ?? null;
                string paramEnabled = httpRequest.Params["enabled"];

                int enabled = paramEnabled != null ? Convert.ToInt32(Convert.ToBoolean(paramEnabled)) : -1;
                int id = paramID != null ? Convert.ToInt32(paramID) : -1;

                Debug.WriteLine($"id=> {id}");
                Debug.WriteLine($"name=> {name}");
                Debug.WriteLine($"type=> {type}");
                Debug.WriteLine($"enabled=> {enabled}");

                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_getProductList", conn))
                    {
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

                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                productList.Add(new Product
                                {
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
                            Debug.WriteLine($"productList=> {JsonConvert.SerializeObject(productList)}");
                        }
                        else
                        {
                            result.Set(112, "找不到該商品");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }

        /// <summary>
        /// 批次搜尋商品清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getProductListByID")]
        public string GetProductListByID([FromBody] GetProductListByIDPayload payload)
        {
            Result result = new Result(100, "缺少參數");
            List<Product> productList = new List<Product> { };

            Debug.WriteLine($"payload=> {JsonConvert.SerializeObject(payload)}");
            if (payload == null || payload.productIdList == null)
            {
                return result.Stringify();
            }

            // 帶空陣列進來直接回空陣列結束
            if (payload.productIdList.Count == 0)
            {
                result.Set(200, "success", productList);
            }

            try
            {
                List<int> productIDList = payload.productIdList;

                // List轉成datatable
                DataTable productDt = new DataTable();
                productDt.Columns.Add("ID", typeof(int));
                for (int i = 0; i < productIDList.Count; i++)
                {
                    productDt.Rows.Add(productIDList[i]);
                }

                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_getProductListByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ids", productDt);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                productList.Add(new Product
                                {
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
                            Debug.WriteLine($"productList=> {JsonConvert.SerializeObject(productList)}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }
    }
}
