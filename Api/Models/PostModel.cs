using DAL.Entities;

namespace Api.Models
{
    public class PostModel
    {
        public Guid UserId { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
