namespace Api.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public string? Model { get; set; }
        public string? Name { get; set; }
        public override string Message => $"{Name} {Model} already exist";
    }

    public class UserAlreadyExistException : AlreadyExistException
    {
        public UserAlreadyExistException(string name)
        {
            Model = "User";
            Name = name;
        }
    }

    public class SubscribtionAlreadyExistException : AlreadyExistException
    {
        public SubscribtionAlreadyExistException()
        {
            Model = "Subscribtion";
            Name = null;
        }
    }

    public class LikeAlreadyExistException : AlreadyExistException
    {
        public LikeAlreadyExistException()
        {
            Model = "Like";
            Name = null;
        }
    }
}
