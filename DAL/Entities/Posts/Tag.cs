using DAL.Entities.Users;

namespace DAL.Entities.Posts
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;

        public virtual User Author { get; set; } = null!;
        public virtual ICollection<Post>? Posts { get; set; }
    }
}
