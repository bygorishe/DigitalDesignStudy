using DAL.Entities.UserAssociations;

namespace DAL.Entities.PostAssociations
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
    }
}
