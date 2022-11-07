using DAL.Entities;

namespace Api.Models
{
    public class CommentModel
    {
        public string Caption { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }

        public virtual User Author { get; set; } = null!;
    }
}
