using DAL.Entities;

namespace Api.Models
{
    public class CreatePostModel
    {
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
