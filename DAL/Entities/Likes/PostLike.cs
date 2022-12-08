using DAL.Entities.Posts;
using System;
namespace DAL.Entities.Likes
{
    public class PostLike : Like
    {
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
    }
}
