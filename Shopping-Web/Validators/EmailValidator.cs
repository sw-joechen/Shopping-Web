using System.Text.RegularExpressions;

namespace Shopping_Web.Validators
{
    public class EmailValidator
    {
        public bool IsEmailValid(string email) {
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }
    }
}