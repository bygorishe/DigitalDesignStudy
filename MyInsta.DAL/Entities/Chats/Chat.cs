using MyInsta.DAL.Entities.Attaches;
using MyInsta.DAL.Entities.Users;

namespace MyInsta.DAL.Entities.Chats
{
    public class Chat
    {
        public Guid Id { get; set; }
        public Guid AdminId { get; set; }
        public string Name { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeleteDate { get; set; }

        public virtual Avatar? Avatar { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
