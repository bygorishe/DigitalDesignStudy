using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.User;

namespace Api.Models.Post
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public UserModel Author { get; set; } = null!;
        public List<AttachWithLinkModel> Images { get; set; } = new List<AttachWithLinkModel>();
        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();

    }
}
