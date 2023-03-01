using MyInsta.DAL.Entities.Attaches;
using MyInsta.DAL.Entities.Likes;
using MyInsta.DAL.Entities.Users;

namespace MyInsta.DAL.Entities.Posts
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? DeleteDate { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual ICollection<PostImage> PostImages { get; set; } = null!;
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<PostLike>? Likes { get; set; }
        public virtual ICollection<Tag>? Tags { get; set; }
    }
}
