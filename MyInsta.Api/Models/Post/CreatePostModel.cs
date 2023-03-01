using MyInsta.Api.Models.Attach;

namespace MyInsta.Api.Models.Post
{
    public class CreatePostModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public List<MetaLinkModel> Contents { get; set; } = new List<MetaLinkModel>();
    }

    public class CreatePostRequest
    {
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
        public List<MetadataModel> Contents { get; set; } = new List<MetadataModel>();
    }
}
