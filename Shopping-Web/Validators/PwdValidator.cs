using System.Text.RegularExpressions;

namespace Shopping_Web.Validators {
    public class PwdValidator {
        public bool IsPwdValid(string pwd) {
            // 密碼規則: 6 位數以上，並且至少包含大寫字母、小寫字母、數字各一
            return Regex.IsMatch(pwd, "^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])[a-zA-Z0-9!@#$%^&*]{6,}$");
        }
    }
}