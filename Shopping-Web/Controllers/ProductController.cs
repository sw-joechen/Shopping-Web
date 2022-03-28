using Shopping_Web.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

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
                                    id = r["f_id"].ToString(),
                                    name = r["f_name"].ToString(),
                                    description = r["f_description"].ToString(),
                                    price = Convert.ToInt16(r["f_price"]),
                                    picture = $"{httpRequest.Url.Scheme}://{httpRequest.Url.Authority}/Uploads{r["f_picture"].ToString()}",
                                    amount = Convert.ToInt16(r["f_amount"]),
                                    type = r["f_type"].ToString(),
                                    enabled = Convert.ToBoolean(Convert.ToInt16(r["f_enabled"])),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
                                });
                            }
                            result.Set(200, "success", productList);
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
    }
}
