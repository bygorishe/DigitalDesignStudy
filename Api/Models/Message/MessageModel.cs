using Api.Models.User;

namespace Api.Models.Message
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public UserAvatarModel User { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
    }
}
