using DAL.Entities.Chats;

namespace DAL.Entities.Likes
{
    public class MessageLike : Like
    {
        public virtual Message Message { get; set; } = null!;
    }
}
