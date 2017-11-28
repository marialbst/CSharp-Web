using System.Text.RegularExpressions;
using WebServer.GameStore.ViewModels.Account;

namespace WebServer.GameStore.Validation
{
    public static class ValidateInput
    {
        public static string Validate(RegisterViewModel model)
        {
            if (!ValidateEmail(model.Email))
            {
                return ValidationErrors.EmailError;
            }
            else if (!ValidatePasword(model.Password))
            {
                return ValidationErrors.PasswordError;
            }
            else if(!ValidatePasswordMatch(model.Password, model.ConfirmPassword))
            {
                return ValidationErrors.PasswordConfirmationError;
            }

            return string.Empty;
        }

        private static bool ValidateEmail(string input)
        {
            return input.Contains("@") && input.Contains(".");
        }

        private static bool ValidatePasword(string input)
        {
            return Regex.IsMatch(input, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$");
        }

        private static bool ValidatePasswordMatch(string password, string confirmedPassword)
        {
            return password.Equals(confirmedPassword);
        }
    }
}
