namespace Api.Models.Message
{
    public class CreateMessageModel
    {
        public Guid? UserId { get; set; }
        public Guid ChatId { get; set; }
        public string Text { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
    }
}
