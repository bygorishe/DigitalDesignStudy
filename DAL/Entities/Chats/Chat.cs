using DAL.Entities.Users;

namespace DAL.Entities.Chats
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        //public virtual Avatar? Avatar { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeleteDate { get; set; }
    }
}
