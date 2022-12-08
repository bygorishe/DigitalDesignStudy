namespace DAL.Entities.Users
{
    public class Subscribtion
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FollowerId { get; set; }
        public DateTimeOffset SubscribeTime { get; set; }
        public bool IsCanceled { get; set; } = false;
        public DateTimeOffset? CancelTime { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual User Follower { get; set; } = null!;
    }
}
