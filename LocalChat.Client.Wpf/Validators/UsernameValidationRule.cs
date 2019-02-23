using System.Globalization;
using System.Windows.Controls;

namespace LocalChat.Client.Wpf.Validators
{
    public class UsernameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool isEmpty = string.IsNullOrWhiteSpace((value ?? "").ToString());
            if (isEmpty)
                return new ValidationResult(false, "Field is required.");

            if (value.ToString().Contains(":"))
                return new ValidationResult(false, "Username must not contains a ':' symbol.");

            return ValidationResult.ValidResult;
        }
    }
}
