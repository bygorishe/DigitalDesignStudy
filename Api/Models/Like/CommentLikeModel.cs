using Api.Models.User;

namespace Api.Models.Like
{
    public class CommentLikeModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public virtual UserModel User { get; set; } = null!;
    }
}
