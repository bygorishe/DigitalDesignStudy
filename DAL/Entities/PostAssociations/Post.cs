using DAL.Entities.AttachAssociations;
using DAL.Entities.UserAssociations;

namespace DAL.Entities.PostAssociations
{
    public class Post
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual ICollection<PostImage> PostImages { get; set; } = null!;
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
    }
}
