using MyInsta.DAL.Entities.Posts;

namespace MyInsta.DAL.Entities.Attaches
{
    public class PostImage : Attach
    {
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
    }
}
