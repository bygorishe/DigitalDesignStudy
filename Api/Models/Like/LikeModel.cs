using Api.Models.User;

namespace Api.Models.Like
{
    public class LikeModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public virtual UserAvatarModel User { get; set; } = null!;
    }
}
