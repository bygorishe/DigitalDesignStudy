using DAL.Entities.Attaches;
using DAL.Entities.Users;
using DAL.Entities.Likes;

namespace DAL.Entities.Chats
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public string Text { get; set; } = null!;
        public Attach? Image { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<MessageLike>? Likes { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeleteDate { get; set; }
    }
}
