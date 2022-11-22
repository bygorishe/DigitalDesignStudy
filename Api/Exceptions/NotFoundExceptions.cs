namespace Api.Exceptions
{
    public class NotFoundException : Exception
    {
        public string? Model { get; set; }
        public override string Message => $"{Model} is not found";
    }

    public class FileNotFoundException : NotFoundException
    {
        public FileNotFoundException()
        {
            Model = "File";
        }
    }

    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException()
        {
            Model = "User";
        }
    }

    public class PostNotFoundException : NotFoundException
    {
        public PostNotFoundException()
        {
            Model = "Post";
        }
    }

    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException()
        {
            Model = "Comment";
        }
    }

    public class LikeNotFoundException : NotFoundException
    {
        public LikeNotFoundException()
        {
            Model = "Like";
        }
    }

    public class SubscridtionNotFoundException : NotFoundException
    {
        public SubscridtionNotFoundException()
        {
            Model = "Subscridtion";
        }
    }

    public class SessionNotFoundException : NotFoundException
    {
        public SessionNotFoundException()
        {
            Model = "Session";
        }
    }
}
