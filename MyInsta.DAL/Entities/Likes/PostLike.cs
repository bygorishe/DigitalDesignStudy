using MyInsta.DAL.Entities.Posts;

namespace MyInsta.DAL.Entities.Likes
{
    public class PostLike : Like
    {
        public Guid PostId { get; set; }

        public virtual Post Post { get; set; } = null!;
    }
}
