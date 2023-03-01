using MyInsta.DAL.Entities.Chats;

namespace MyInsta.DAL.Entities.Likes
{
    public class MessageLike : Like
    {
        public Guid MessageId { get; set; }

        public virtual Message Message { get; set; } = null!;
    }
}
