using DAL.Entities;

namespace Api.Models
{
    public class PostModel
    {
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual ICollection<PostImage> PostImages { get; set; } = null!;
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
