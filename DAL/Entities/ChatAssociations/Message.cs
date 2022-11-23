using DAL.Entities.UserAssociations;

namespace DAL.Entities.ChatAssociations
{
    public class Message
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public virtual User User { get; set; } = null!;
        //public bool IsRead { get; set; }
    }
}
