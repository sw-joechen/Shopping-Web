using System.Text.RegularExpressions;

namespace Shopping_Web.Validators
{
    public class SpecialCharacterValidator
    {
        public bool IsStrContainSpecialCharacter(string str)
        {
            // 過濾特殊字元(包含空格)
            return Regex.IsMatch(str, "[\\s`~!@#$%^&*\"()_+\\-=[\\]{};':\\|,.<>\\/?]+");
        }
    }
}