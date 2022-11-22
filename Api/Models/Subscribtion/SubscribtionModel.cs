namespace Api.Models.Subscribtion
{
    public class SubscribtionModel
    {
        public Guid? UserId { get; set; }
        public Guid FollowerId { get; set; }
        public DateTimeOffset SubscribeTime { get; set; }
    }
}
