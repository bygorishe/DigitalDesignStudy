namespace Api.Exceptions
{
    public class WrongPasswordException : Exception
    {
        public override string Message => $"Password is incorrect";
    }
    public class NotEqualsPasswordsException : Exception
    {
        public override string Message => $"Passwords is equals";
    }

    public class NotAuthorizedException : Exception
    {
        public override string Message => $"You are not authorized";
    }

    public class NotVerifiedException : Exception
    {
        public override string Message => $"You are not verified";
    }
}
