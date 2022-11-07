namespace Api.Models
{
    public class AddPostImage
    {
        public MetadataModel PostImage { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
