using Api.Models.User;

namespace Api.Models.Like
{
    public class LikeModel
    {
        public DateTimeOffset CreatedDate { get; set; }
        public virtual UserAvatarModel User { get; set; } = null!;
    }
}
