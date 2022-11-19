using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.Like;
using Api.Models.User;

namespace Api.Models.Post
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public UserModel Author { get; set; } = null!;
        public List<AttachExternalModel> Contens { get; set; } = new List<AttachExternalModel>();
        public List<CommentModel>? Comments { get; set; } = new List<CommentModel>();
        public List<LikeModel>? LikeModels { get; set; } = new List<LikeModel>();
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLiked { get; set; }
    }
}
