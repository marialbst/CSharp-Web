namespace GameStore.App.Infrastructure
{
    public static class ErrorConstants
    {
        public const string RegisterError = @"<strong>Invalid form data!</strong>"
                   + "<ul class=\"text-left\">"
                   + "   <li>Password should be at least 6 symbols long, containing 1 uppercase letter, 1 lowercase letter and 1 digit.</li>"
                   + "   <li>Confirm password should match provided password</li>"
                   + "   <li>Email must contain '@' and '.'</li>"
                   + "</ul>";
        public const string ExistingUserError = "Username already taken";

        public const string LoginError = "<p>Both fields are required!</p>";

        public const string InvalidCredentials = "<p>Invalid username or password!</p>";
    }
}
