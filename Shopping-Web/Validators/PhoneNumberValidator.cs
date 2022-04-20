using System.Text.RegularExpressions;

namespace Shopping_Web.Validators {
    public class PhoneNumberValidator {
        public bool IsPhoneNumberValid(string phoneNumber) {
            return Regex.IsMatch(phoneNumber, @"^09[0-9]{8}$");
        }
    }
}