namespace Api.Models
{
    public class CreateCommentModel
    {
        public string Caption { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
    }
}
