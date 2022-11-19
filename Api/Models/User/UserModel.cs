namespace Api.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? FullName { get; set; }
        public string? About { get; set; }
        public string Email { get; set; } = null!;
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset RegistrateDate { get; set; }
        public int PostsCount { get; set; }
        public int FollowersCount { get; set; }
        public int SubscribtionsCount { get; set; }
    }

    public class UserAvatarModel : UserModel
    {
        public string? AvatarLink { get; set; }
    }

    //public class SubUserModel : UserModel
    //{
    //    public bool IsSubscribe { get; set; }
    //}
}
