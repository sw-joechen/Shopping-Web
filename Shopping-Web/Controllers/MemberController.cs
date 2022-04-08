using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
    }
}