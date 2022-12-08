using DAL.Entities.Posts;

namespace DAL.Entities.Likes
{
    public class CommentLike : Like
    {
        public Guid CommentId { get; set; }
        public virtual Comment Comment { get; set; } = null!;
    }
}
