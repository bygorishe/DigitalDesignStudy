using Api.Models.Attach;

namespace Api.Models.Post
{
    public class CreatePostModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<MetaWithPath> Contents { get; set; } = new List<MetaWithPath>();
    }

    public class CreatePostRequest
    {
        public string? Description { get; set; }
        public List<MetadataModel> Contents { get; set; } = new List<MetadataModel>();
    }
}
