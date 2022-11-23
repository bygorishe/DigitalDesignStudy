using Api.Models.Message;
using Api.Models.User;

namespace Api.Models.Chat
{
    public class ChatModel
    {
        public string? Name { get; set; }
        public List<UserAvatarModel> Users { get; set; } = new();
        public List<MessageModel> Messages { get; set; } = new();
        public DateTimeOffset CreatedDate { get; set; }
        public int MessagesCount { get; set; }
        public int MembersCount { get; set; }
    }
}
