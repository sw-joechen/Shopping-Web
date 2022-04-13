﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Shopping_Web.Class;
using Shopping_Web.Models;
using Shopping_Web.Validators;

namespace Shopping_Web.Controllers
{
    public class MemberController : ApiController
    {
        readonly string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        /// <summary>
        /// 註冊帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/registerMember")]
        public string RegisterMember()
        {
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Params["account"] == null || httpRequest.Params["pwd"] == null || httpRequest.Params["address"] == null || 
                httpRequest.Params["phone"] == null || httpRequest.Params["gender"] == null || httpRequest.Params["email"] == null )
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];
                string pwd = httpRequest.Params["pwd"];
                string address = httpRequest.Params["address"];
                string phone = httpRequest.Params["phone"];
                int gender = Convert.ToInt32(httpRequest.Params["gender"]);
                string email = httpRequest.Params["email"];

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"pwd=> {pwd}");
                Debug.WriteLine($"address=> {address}");
                Debug.WriteLine($"phone=> {phone}");
                Debug.WriteLine($"gender=> {gender}");
                Debug.WriteLine($"email=> {email}");

                AccountValidator accValidator = new AccountValidator();
                PwdValidator pwdValidator = new PwdValidator();
                SpecialCharacterValidator specialCharacterValidator = new SpecialCharacterValidator();
                EmailValidator emailValidator = new EmailValidator();
                PhoneNumberValidator phoneNumberValidator = new PhoneNumberValidator();

                // 檢查帳號
                if (!accValidator.IsAccountValid(account))
                {
                    result.Set(103, "帳號密碼不符合規則");
                    return result.Stringify();
                }

                // 檢查密碼
                if (!pwdValidator.IsPwdValid(pwd))
                {
                    result.Set(103, "帳號密碼不符合規則");
                    return result.Stringify();
                }

                // 檢查email
                if (!emailValidator.IsEmailValid(email))
                {
                    result.Set(116, "email格式不合法");
                    return result.Stringify();
                }

                // 檢查phone
                if (!phoneNumberValidator.IsPhoneNumberValid(phone))
                {
                    result.Set(117, "電話號碼格式不合法");
                    return result.Stringify();
                }

                // Hash password
                string hashedPwd = SecurePasswordHasher.Hash(pwd);

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    int sqlResponse = 0;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_addMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@pwd", hashedPwd);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@enabled", 1);
                        cmd.Parameters.AddWithValue("@balance", 0);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            sqlResponse = (int)r["result"];
                        }
                    }

                    if (sqlResponse == 200)
                    {
                        result.Set(200, "success");
                    }
                    else
                    {
                        result.Set(118, "帳號已經存在");
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
        /// 更新帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/updateMember")]
        public string UpdateMember()
        {
            var dict = new Dictionary<string, object>();
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Params["account"] == null || httpRequest.Params["address"] == null || httpRequest.Params["phone"] == null || 
                httpRequest.Params["gender"] == null || httpRequest.Params["email"] == null)
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];
                string address = httpRequest.Params["address"];
                string phone = httpRequest.Params["phone"];
                int gender = Convert.ToInt32(httpRequest.Params["gender"]);
                string email = httpRequest.Params["email"];

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"address=> {address}");
                Debug.WriteLine($"phone=> {phone}");
                Debug.WriteLine($"gender=> {gender}");
                Debug.WriteLine($"email=> {email}");

                AccountValidator accValidator = new AccountValidator();
                PwdValidator pwdValidator = new PwdValidator();
                SpecialCharacterValidator specialCharacterValidator = new SpecialCharacterValidator();
                EmailValidator emailValidator = new EmailValidator();
                PhoneNumberValidator phoneNumberValidator = new PhoneNumberValidator();

                // 檢查帳號
                if (!accValidator.IsAccountValid(account))
                {
                    result.Set(103, "帳號密碼不符合規則");
                    return result.Stringify();
                }

                // 檢查email
                if (!emailValidator.IsEmailValid(email))
                {
                    result.Set(116, "email格式不合法");
                    return result.Stringify();
                }

                // 檢查phone
                if (!phoneNumberValidator.IsPhoneNumberValid(phone))
                {
                    result.Set(117, "電話號碼格式不合法");
                    return result.Stringify();
                }

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_editMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@email", email);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            dict["id"] = r["f_id"];
                            dict["account"] = r["f_account"];
                            dict["address"] = r["f_address"];
                            dict["phone"] = r["f_phone"];
                            dict["gender"] = r["f_gender"];
                            dict["email"] = r["f_email"];
                            dict["enabled"] = r["f_enabled"];
                            dict["balance"] = r["f_balance"];
                            dict["createdDate"] = r["f_createdDate"];
                            dict["updatedDate"] = r["f_updatedDate"];
                            result.Set(200, "success", dict);
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
        /// 更新帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/updateMemberPwd")]
        public string updateMemberPwd()
        {
            var dict = new Dictionary<string, object>();
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Params["account"] == null || httpRequest.Params["oldPwd"] == null || httpRequest.Params["newPwd"] == null)
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];
                string oldPwd = httpRequest.Params["oldPwd"];
                string newPwd = httpRequest.Params["newPwd"];

                // Hash new password
                string hashedNewPwd = SecurePasswordHasher.Hash(newPwd);

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"oldPwd=> {oldPwd}");
                Debug.WriteLine($"newPwd=> {newPwd}");

                PwdValidator pwdValidator = new PwdValidator();

                // 進庫撈使用者
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_getMemberByAccount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            dict["id"] = r["f_id"];
                            dict["account"] = r["f_account"];
                            dict["pwd"] = r["f_pwd"];
                            dict["address"] = r["f_address"];
                            dict["phone"] = r["f_phone"];
                            dict["gender"] = r["f_gender"];
                            dict["email"] = r["f_email"];
                            dict["enabled"] = r["f_enabled"];
                            dict["balance"] = r["f_balance"];
                            dict["createdDate"] = r["f_createdDate"];
                            dict["updatedDate"] = r["f_updatedDate"];
                        }
                        else
                        {
                            result.Set(105, "帳號錯誤");
                            return result.Stringify();
                        }
                        // 關閉連線
                        conn.Close();
                    }
                }
                Debug.WriteLine($"dict=> {JsonConvert.SerializeObject(dict)}");

                // check status
                if (Convert.ToInt16(dict["enabled"]) == 0)
                {
                    result.Set(108, "該帳號已被禁用");
                    return result.Stringify();
                }

                // check pwd
                bool verify = SecurePasswordHasher.Verify(oldPwd, dict["pwd"].ToString());
                if (verify)
                {
                    // 寫進庫
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("pro_sw_editMemberPwd", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@account", account);
                            cmd.Parameters.AddWithValue("@newPwd", hashedNewPwd);
                            SqlDataReader r = cmd.ExecuteReader();
                            if (r.Read())
                            {
                                result.Set(200, "success");
                            }
                        }
                    }
                }
                else
                {
                    // 密碼錯誤
                    result.Set(104, "密碼錯誤");
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
        /// 登入帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/login")]
        public string Login()
        {
            var dict = new Dictionary<string, object>();
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Params["account"] == null || httpRequest.Params["pwd"] == null)
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];
                string pwd = httpRequest.Params["pwd"];

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"pwd=> {pwd}");

                // Hash password
                string hashedPwd = SecurePasswordHasher.Hash(pwd);

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_getMemberByAccount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            dict["id"] = r["f_id"];
                            dict["account"] = r["f_account"];
                            dict["pwd"] = r["f_pwd"];
                            dict["address"] = r["f_address"];
                            dict["phone"] = r["f_phone"];
                            dict["gender"] = r["f_gender"];
                            dict["email"] = r["f_email"];
                            dict["enabled"] = r["f_enabled"];
                            dict["balance"] = r["f_balance"];
                            dict["createdDate"] = r["f_createdDate"];
                            dict["updatedDate"] = r["f_updatedDate"];
                        }
                        else
                        {
                            result.Set(105, "帳號錯誤");
                            return result.Stringify();
                        }
                        // 關閉連線
                        conn.Close();
                    }
                }

                // check status
                if (Convert.ToInt16(dict["enabled"]) == 0)
                {
                    result.Set(108, "該帳號已被禁用");
                    return result.Stringify();
                }

                //check pwd
                bool verify = SecurePasswordHasher.Verify(pwd, dict["pwd"].ToString());
                if (verify)
                {
                    result.Set(200, "success", dict);
                }
                else
                {
                    // 密碼錯誤
                    result.Set(104, "密碼錯誤");
                }
            }
            catch (Exception ex)
            {
                // test
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }

            return result.Stringify();
        }

        /// <summary>
        /// 取得會員個人資訊
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getMemberPersonalInfo")]
        public string GetMemberPersonalInfo()
        {
            var httpRequest = HttpContext.Current.Request;
            Result result = new Result(100, "缺少參數");
            List<Member> memberList = new List<Member> { };

            if (httpRequest.Params["account"] == null )
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];

                Debug.WriteLine($"account=> {account}");

                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_getMemberByAccount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                memberList.Add(new Member
                                {
                                    id = Convert.ToInt16(r["f_id"]),
                                    account = r["f_account"].ToString(),
                                    address = r["f_address"].ToString(),
                                    phone = r["f_phone"].ToString(),
                                    gender = Convert.ToInt16(r["f_gender"]),
                                    email = r["f_email"].ToString(),
                                    enabled = Convert.ToBoolean(Convert.ToInt16(r["f_enabled"])),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
                                    balance = Convert.ToDouble(r["f_balance"]),
                                    pwd = r["f_pwd"].ToString()
                                });
                            }

                        }
                        result.Set(200, "success", memberList);
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
        /// 結帳
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/purchase")]
        public async Task<string> Purchase([FromBody] PurchasePayload payload)
        {
            Result result = new Result(100, "缺少參數");
            List<Product> productList = new List<Product> { };
            Dictionary<int, int> shoppingCartDict = new Dictionary<int, int>();

            if (payload == null || payload.account == null || payload.shoppingList == null)
            {
                return result.Stringify();
            }

            try
            {
                string account = payload.account;
                List<ShoppingItem> shoppingList = payload.shoppingList;                

                Debug.WriteLine($"payload=> {payload}");
                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"shoppingList=> {JsonConvert.SerializeObject(shoppingList)}");

                AccountValidator accountValidator = new AccountValidator();
                // 檢查帳號
                if (!accountValidator.IsAccountValid(account))
                {
                    result.Set(103, "account not valid");
                    return result.Stringify();
                }

                // 取得商品列表
                // TODO: 改成呼叫批次指定商品id的接口
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_getProductList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", DBNull.Value);
                        cmd.Parameters.AddWithValue("@name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@type", DBNull.Value);
                        cmd.Parameters.AddWithValue("@enabled", DBNull.Value);
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
                        }
                    }
                    Debug.WriteLine($"productList=> {JsonConvert.SerializeObject(productList)}");
                }

                // 檢查購買清單中的商品是否可以被購買
                bool isShoppingListAllValid = false;
                int invalidProductItemID = -1;
                // 0: 狀態錯誤, 1:數量錯誤, 2:id在清單中匹配不到
                int rejectCondition = -1;
                int cash = 0;

                foreach (ShoppingItem cartItem in shoppingList)
                {
                    isShoppingListAllValid = false;
                    bool isFound = false;
                    foreach (Product prod in productList)
                    {
                        if (cartItem.id == prod.id)
                        {
                            isFound = true;
                            if (!prod.enabled)
                            {
                                invalidProductItemID = cartItem.id;
                                rejectCondition = 0;
                                break;
                            }
                            else if (cartItem.count > prod.amount || cartItem.count <= 0)
                            {
                                invalidProductItemID = cartItem.id;
                                rejectCondition = 1;
                                break;
                            }

                            // 商品ok
                            isShoppingListAllValid = true;

                            // 累加消費金額
                            cash += (cartItem.count * prod.price);
                            shoppingCartDict.Add(cartItem.id, cartItem.count);
                            Debug.WriteLine($"{cartItem.id} is valid");
                            break;
                        }
                    }

                    // 購買清單含有未匹配的商品
                    if (!isFound)
                    {
                        invalidProductItemID = cartItem.id;
                        rejectCondition = 2;
                        Debug.WriteLine($"== {cartItem.id} is not matched ==");
                        break;
                    }

                    // 購買清單含有不合法商品(數量,狀態)
                    if (!isShoppingListAllValid || invalidProductItemID != -1)
                    {
                        Debug.WriteLine($"== {cartItem.id} is invalid, break the loop ==");
                        break;
                    }
                }

                if (!isShoppingListAllValid)
                {
                    Object data = new { id = invalidProductItemID };
                    switch (rejectCondition)
                    {
                        case 0:
                            {
                                result.Set(119, "結帳清單包含被禁用的商品", data);
                                break;
                            }
                        case 1:
                            {
                                result.Set(120, "結帳清單包含無效的數量", data);
                                break;
                            }
                        case 2:
                            {
                                result.Set(121, "結帳清單包含無效的商品", data);
                                break;
                            }
                        default:
                            break;

                    }
                    return result.Stringify();
                }
                else
                {
                    // 扣款& 更新商品數量
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        var sqlResponse = 0;
                        //convert to DataTable
                        var dt = ConvertToDataTable(shoppingCartDict);

                        Debug.WriteLine($"cash: {cash}");
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("pro_sw_editMemberAndEditProduct", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@account", account);
                            cmd.Parameters.AddWithValue("@cash", cash);
                            cmd.Parameters.AddWithValue("@shoppingList", dt);
                            SqlDataReader r = cmd.ExecuteReader();
                            if (r.Read())
                            {
                                sqlResponse = (int)r["result"];
                                Debug.WriteLine($"sqlResponse: {sqlResponse}");
                            }
                        }

                        switch (sqlResponse){
                            case 200:
                                AddPurchaseRecord(payload);
                                result.Set(200, "success");
                                break;
                            case 100:
                                result.Set(105, "帳號錯誤");
                                break;
                            case 101:
                                result.Set(108, "該帳號已被禁用");
                                break;
                            case 102:
                                result.Set(122, "餘額不足");
                                break;
                            default:
                                result.Set(101, "網路錯誤");
                                break;
                        }
                    }
                }                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return $"exception=> {e}";
            }
            return result.Stringify();
        }

        private static DataTable ConvertToDataTable(Dictionary<int, int> dict)
        {
            var dt = new DataTable();
            dt.Columns.Add("id", typeof(Int32));
            dt.Columns.Add("count", typeof(Int32));
            foreach (var pair in dict)
            {
                var row = dt.NewRow();
                row["id"] = pair.Key;
                row["count"] = pair.Value;
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// 新增購買記錄
        /// </summary>
        private string AddPurchaseRecord(PurchasePayload payload)
        {
            Result result = new Result(100, "缺少參數");
            Member memberInfo = new Member();
            //List<Product> productList = new List<Product> { };
            //Dictionary<int, int> shoppingCartDict = new Dictionary<int, int>();

            if (payload == null || payload.account == null || payload.shoppingList == null)
            {
                return result.Stringify();
            }

            try
            {
                string account = payload.account;
                List<ShoppingItem> shoppingList = payload.shoppingList;
                DataTable shoppingTd = new DataTable();
                shoppingTd = ToDataTable(shoppingList);

                DateTime dt = DateTime.UtcNow;
                string year = dt.Year.ToString().Substring(2);
                string month = dt.Month.ToString("00");
                string day = dt.Day.ToString("00");
                string hour = dt.Hour.ToString("00");
                string minute = dt.Minute.ToString("00");
                string second = dt.Second.ToString("00");

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"shoppingList=> {JsonConvert.SerializeObject(shoppingList)}");
                Debug.WriteLine($"shoppingTd=> {JsonConvert.SerializeObject(shoppingTd)}");

                // 進庫撈會員資訊
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_getMemberList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@enabled", DBNull.Value);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                memberInfo.id = Convert.ToInt16(r["f_id"]);
                                memberInfo.account = r["f_account"].ToString();
                                memberInfo.address = r["f_address"].ToString();
                                memberInfo.phone = r["f_phone"].ToString();
                                memberInfo.gender = Convert.ToInt16(r["f_gender"]);
                                memberInfo.email = r["f_email"].ToString();
                                memberInfo.enabled = Convert.ToBoolean(Convert.ToInt16(r["f_enabled"]));
                                memberInfo.createdDate = r["f_createdDate"].ToString();
                                memberInfo.updatedDate = r["f_updatedDate"].ToString();
                                memberInfo.balance = Convert.ToDouble(r["f_balance"]);                                
                            }
                        }                        
                    }
                }
                Debug.WriteLine($"memberInfo=> {JsonConvert.SerializeObject(memberInfo)}");
                string accountID = memberInfo.id.ToString("00000000");

                // 組出訂單編號
                string orderNumber = year+month+day+hour+minute+second+accountID;
                string createdDate = dt.ToString("yyyy-MM-ddTHH:mm:sssZ");

                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    // 寫購買紀錄進庫
                    using (SqlCommand cmd = new SqlCommand("pro_sw_addPurchaseRecord", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@phone", memberInfo.phone);
                        cmd.Parameters.AddWithValue("@address", memberInfo.address);
                        cmd.Parameters.AddWithValue("@createdDate", createdDate);
                        SqlDataReader r = cmd.ExecuteReader();
                    }

                    // 寫購買明細進庫
                    using (SqlCommand cmd = new SqlCommand("pro_sw_addSubPurchaseRecord", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@orderNumber", orderNumber);
                        cmd.Parameters.AddWithValue("@shoppingDetail", shoppingTd);
                        SqlDataReader r = cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                result.Set(101, "網路錯誤");
                return result.Stringify();
            }
            return result.Stringify();
        }
    }
}