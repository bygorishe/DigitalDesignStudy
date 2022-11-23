using DAL.Entities.UserAssociations;

namespace DAL.Entities.PostAssociations
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Caption { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }

        public virtual User Author { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
        //public virtual ICollection<Like>? Likes { get; set; }
    }
}
