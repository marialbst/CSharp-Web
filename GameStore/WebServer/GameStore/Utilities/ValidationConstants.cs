namespace WebServer.GameStore.Utilities
{
    public static class ValidationConstants
    {
        public const string EmailError = "Invalid Email. It should contain @ and .";

        public const string PasswordError = "Invalid Password. It should be at least 6 symbols long, containing 1 uppercase letter, 1 lowercase letter and 1 digit.";

        public const string UsernameTakenError = "Username already taken";

        public const string InvalidImageUrl = "Url must start with http://, https:// or be equal to null";

        public const string EmailOrPasswordError = "Invalid login details";

        public const string FirstLetterTitleError = "Title must start with uppercase letter";

        public const string InvalidMinLengthError = "{0} must be at least {1} symbols.";

        public const string InvalidMaxLengthError = "{0} must be less than {1} symbols.";

        public const string InvalidLengthError = "{0} must be exactly {1} symbols";

        public const string InvalidNumberError = "{0} must be greater than {1}";

        public class Account
        {
            public const int EmailMaxLength = 30;

            public const int FullNameMinLength = 2;

            public const int FullNameMaxLength = 30;

            public const int PasswordMinLength = 6;

            public const int PasswordMaxLength = 50;
        }

        public class Game
        {
            public const int DescriptionMinLength = 20;

            public const int TitleMinLength = 3;

            public const int TitleMaxLength = 100;

            public const int TrailerIdLength = 11;
        }
    }
}
