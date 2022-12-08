namespace Api.Exceptions
{
    public class DeletedException : Exception
    {
        public string? Model { get; set; }
        public override string Message => $"{Model} is deleted";
    }

    public class FileDeletedException : DeletedException
    {
        public FileDeletedException()
        {
            Model = "File";
        }
    }

    public class UserDeletedException : DeletedException
    {
        public UserDeletedException()
        {
            Model = "User";
        }
    }

    public class PostDeletedException : DeletedException
    {
        public PostDeletedException()
        {
            Model = "Post";
        }
    }

    public class CommentDeletedException : DeletedException
    {
        public CommentDeletedException()
        {
            Model = "Comment";
        }
    }

    public class LikeDeletedException : DeletedException
    {
        public LikeDeletedException()
        {
            Model = "Like";
        }
    }

    public class SubscridtionDeletedException : DeletedException
    {
        public SubscridtionDeletedException()
        {
            Model = "Subscridtion";
        }
    }

    public class SessionDeletedException : DeletedException
    {
        public SessionDeletedException()
        {
            Model = "Session";
        }
    }

    public class ChatDeletedException : DeletedException
    {
        public ChatDeletedException()
        {
            Model = "Chat";
        }
    }

    public class MessageDeletedException : DeletedException
    {
        public MessageDeletedException()
        {
            Model = "Message";
        }
    }
}
