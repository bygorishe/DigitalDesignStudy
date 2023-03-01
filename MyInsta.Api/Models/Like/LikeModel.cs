using MyInsta.Api.Models.User;

namespace MyInsta.Api.Models.Like
{
    public class LikeModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public virtual UserAvatarModel User { get; set; } = null!;
    }
}
