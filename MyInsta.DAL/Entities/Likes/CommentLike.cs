using MyInsta.DAL.Entities.Posts;

namespace MyInsta.DAL.Entities.Likes
{
    public class CommentLike : Like
    {
        public Guid CommentId { get; set; }

        public virtual Comment Comment { get; set; } = null!;
    }
}
