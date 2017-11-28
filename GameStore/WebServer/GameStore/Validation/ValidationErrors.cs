namespace WebServer.GameStore.Validation
{
    public static class ValidationErrors
    {
        public const string EmailError = "Invalid Email. It should contain @ and .";

        public const string PasswordError = "Invalid Password. It should be at least 6 symbols long, containing 1 uppercase letter, 1 lowercase letter and 1 digit.";

        public const string PasswordConfirmationError = "Passwords doesn't match. Confirmed password must be same as Password";

        public const string UsernameTakenError = "Username already taken";

        public const string EmailOrPasswordError = "Invalid login details";
    }
}
