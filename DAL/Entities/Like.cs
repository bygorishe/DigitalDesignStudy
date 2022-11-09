namespace DAL.Entities
{
    public class Like
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
    }
}
