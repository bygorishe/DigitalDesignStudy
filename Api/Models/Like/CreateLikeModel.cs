namespace Api.Models.Like
{
    public class CreateLikeModel
    {
        public Guid? UserId { get; set; }
        public Guid ObjectId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
