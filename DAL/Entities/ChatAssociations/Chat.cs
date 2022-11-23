using DAL.Entities.AttachAssociations;
using DAL.Entities.UserAssociations;

namespace DAL.Entities.ChatAssociations
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        //public virtual Avatar? Avatar { get; set; }
        //public Guid MainUserId { get; set; }
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public DateTimeOffset CreatedDate { get; set; }
    }
}
