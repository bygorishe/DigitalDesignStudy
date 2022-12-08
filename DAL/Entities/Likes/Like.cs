using DAL.Entities.Users;

namespace DAL.Entities.Likes
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsCanceled { get; set; } = false;
        public DateTimeOffset? CancelDate { get; set; }

        public virtual User Author { get; set; } = null!;
    }
}
