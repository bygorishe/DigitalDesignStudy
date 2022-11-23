using DAL.Entities.UserAssociations;

namespace DAL.Entities.PostAssociations
{
    public class CommentLike
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public virtual Comment Comment { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
    }
}
