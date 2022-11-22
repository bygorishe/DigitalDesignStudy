namespace Api.Exceptions
{
    public class WrongPasswordException : Exception
    {
        public override string Message => $"Password is incorrect";
    }

    public class NotAuthorizedException : Exception
    {
        public override string Message => $"You are not authorized";
    }
}
