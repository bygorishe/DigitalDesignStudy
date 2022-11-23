namespace Api.Models.Chat
{
    public class CreateChatModel
    {
        public string? Name { get; set; }
        public List<Guid> UsersId { get; set; } = new();
        public DateTimeOffset CreatedDate { get; set; }
    }
}
