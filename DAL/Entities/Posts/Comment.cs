using DAL.Entities.Likes;
using DAL.Entities.Users;

namespace DAL.Entities.Posts
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Caption { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeleteDate { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<CommentLike>? Likes { get; set; }
    }
}
