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
        public UserAvatarModel Author { get; set; } = null!;
        public List<AttachExternalModel> Contens { get; set; } = new List<AttachExternalModel>();
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsLiked { get; set; }
        //public List<CommentModel>? Comments { get; set; } = new List<CommentModel>();
        //public List<LikeModel>? Likes { get; set; } = new List<LikeModel>();
    }

    //public class ExtPostModel : PostModel
    //{
    //    public List<CommentModel>? Comments { get; set; } = new List<CommentModel>();
    //    public List<LikeModel>? Likes { get; set; } = new List<LikeModel>();
    //}
}
